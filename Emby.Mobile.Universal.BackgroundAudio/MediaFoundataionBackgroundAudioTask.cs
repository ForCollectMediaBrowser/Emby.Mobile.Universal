using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Universal.BackgroundAudio.Messages;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace Emby.Mobile.Universal.BackgroundAudio
{
    public sealed class MediaFoundataionBackgroundAudioTask : IBackgroundTask
    {

        private MediaPlaybackList _playlist = new MediaPlaybackList();
        private BackgroundTaskDeferral _deferral;
        private TransportControlsService _transportService;
        private AppState _foregroundAppState = AppState.Unknown;
        private ManualResetEvent _backgroundTaskStarted = new ManualResetEvent(false);
        private bool _playbackStartedPreviously = false;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("Background Audio Task " + taskInstance.Task.Name + " starting...");

            var value = ApplicationSettingsHelper.ReadAndRemoveSettingsValue(BackgroundAudioConstants.AppState) as string;
            if (value == null)
                _foregroundAppState = AppState.Unknown;
            else
                _foregroundAppState = value.ToEnum<AppState>();

            BackgroundMediaPlayer.Current.CurrentStateChanged += Current_CurrentStateChanged;
            BackgroundMediaPlayer.MessageReceivedFromForeground += BackgroundMediaPlayer_MessageReceivedFromForeground;

            _transportService = new TransportControlsService();
            _transportService.TransportControlsButtonPressed += TransportService_TransportControlsButtonPressed;

            if (_foregroundAppState != AppState.Suspended)
                MessageService.SendMessageToForeground(new BackgroundAudioTaskStartedMessage());

            ApplicationSettingsHelper.SaveSettingsValue(BackgroundAudioConstants.BackgroundAudioTaskState, BackgroundTaskState.Running.ToString());

            _deferral = taskInstance.GetDeferral();
            _backgroundTaskStarted.Set();

            taskInstance.Task.Completed += TaskCompleted;
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
        }

        private void TaskCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            _deferral.Complete();
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            try
            {
                _backgroundTaskStarted.Reset();

                ApplicationSettingsHelper.SaveSettingsValue(BackgroundAudioConstants.TrackId, GetCurrentTrackId() == null ? null : GetCurrentTrackId().ToString());
                ApplicationSettingsHelper.SaveSettingsValue(BackgroundAudioConstants.Position, BackgroundMediaPlayer.Current.Position.ToString());
                ApplicationSettingsHelper.SaveSettingsValue(BackgroundAudioConstants.BackgroundAudioTaskState, BackgroundTaskState.Canceled.ToString());
                ApplicationSettingsHelper.SaveSettingsValue(BackgroundAudioConstants.AppState, Enum.GetName(typeof(AppState), _foregroundAppState));

                if (_playlist != null)
                {
                    _playlist.CurrentItemChanged -= Playlist_CurrentItemChanged;
                    _playlist = null;
                }

                BackgroundMediaPlayer.MessageReceivedFromForeground -= BackgroundMediaPlayer_MessageReceivedFromForeground;
                _transportService.TransportControlsButtonPressed -= TransportService_TransportControlsButtonPressed;
                _transportService.Unregister();

                BackgroundMediaPlayer.Shutdown();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            _deferral.Complete();
        }

        private void StartPlayback()
        {
            try
            {
                if (!_playbackStartedPreviously)
                {
                    _playbackStartedPreviously = true;

                    var currentTrackId = ApplicationSettingsHelper.ReadAndRemoveSettingsValue(BackgroundAudioConstants.TrackId);
                    var currentTrackPosition = ApplicationSettingsHelper.ReadAndRemoveSettingsValue(BackgroundAudioConstants.Position);
                    if (currentTrackId != null)
                    {
                        var index = _playlist.Items.ToList().FindIndex(item => GetTrackId(item).ToString() == (string)currentTrackId);

                        if (currentTrackPosition == null)
                        {
                            Debug.WriteLine("StartPlayback: Switching to track " + index);
                            _playlist.MoveTo((uint)index);
                            BackgroundMediaPlayer.Current.Play();
                        }
                        else
                        {
                            TypedEventHandler<MediaPlaybackList, CurrentMediaPlaybackItemChangedEventArgs> handler = null;
                            handler = (MediaPlaybackList list, CurrentMediaPlaybackItemChangedEventArgs args) =>
                            {
                                if (args.NewItem == _playlist.Items[index])
                                {
                                    _playlist.CurrentItemChanged -= handler;

                                    var position = TimeSpan.Parse((string)currentTrackPosition);
                                    Debug.WriteLine("StartPlayback: Setting Position " + position);
                                    BackgroundMediaPlayer.Current.Position = position;
                                    BackgroundMediaPlayer.Current.Play();
                                }
                            };
                            _playlist.CurrentItemChanged += handler;

                            Debug.WriteLine("StartPlayback: Switching to track " + index);
                            _playlist.MoveTo((uint)index);
                        }
                    }
                    else
                    {
                        BackgroundMediaPlayer.Current.Play();
                    }
                }
                else
                {
                    BackgroundMediaPlayer.Current.Play();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void SkipToPrevious()
        {
            _transportService.SetStatus(MediaPlaybackStatus.Changing);
            _playlist.MovePrevious();
            BackgroundMediaPlayer.Current.Play();
        }

        private void SkipToNext()
        {
            _transportService.SetStatus(MediaPlaybackStatus.Changing);
            _playlist.MoveNext();
            BackgroundMediaPlayer.Current.Play();
        }

        void Current_CurrentStateChanged(MediaPlayer sender, object args)
        {
            switch (sender.CurrentState)
            {
                case MediaPlayerState.Playing:
                    _transportService.SetStatus(MediaPlaybackStatus.Playing);
                    break;
                case MediaPlayerState.Paused:
                    _transportService.SetStatus(MediaPlaybackStatus.Paused);
                    break;
                case MediaPlayerState.Closed:
                    _transportService.SetStatus(MediaPlaybackStatus.Closed);
                    break;
            }
        }

        void BackgroundMediaPlayer_MessageReceivedFromForeground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            AppSuspendedMessage appSuspendedMessage;
            if (MessageService.TryParseMessage(e.Data, out appSuspendedMessage))
            {
                _foregroundAppState = AppState.Suspended;
                var currentTrackId = GetCurrentTrackId();
                ApplicationSettingsHelper.SaveSettingsValue(BackgroundAudioConstants.TrackId, currentTrackId == null ? null : currentTrackId.ToString());
                return;
            }

            AppResumedMessage appResumedMessage;
            if (MessageService.TryParseMessage(e.Data, out appResumedMessage))
            {
                _foregroundAppState = AppState.Active;
                return;
            }

            StartPlaybackMessage startPlaybackMessage;
            if (MessageService.TryParseMessage(e.Data, out startPlaybackMessage))
            {
                StartPlayback();
                return;
            }

            SkipNextMessage skipNextMessage;
            if (MessageService.TryParseMessage(e.Data, out skipNextMessage))
            {
                SkipToNext();
                return;
            }

            SkipPreviousMessage skipPreviousMessage;
            if (MessageService.TryParseMessage(e.Data, out skipPreviousMessage))
            {
                SkipToPrevious();
                return;
            }

            TrackChangedMessage trackChangedMessage;
            if (MessageService.TryParseMessage(e.Data, out trackChangedMessage))
            {
                var index = _playlist.Items.ToList().FindIndex(i => (string)i.Source.CustomProperties[BackgroundAudioConstants.TrackId] == trackChangedMessage.Id);
                _transportService.SetStatus(MediaPlaybackStatus.Changing);
                _playlist.MoveTo((uint)index);
                BackgroundMediaPlayer.Current.Play();
                return;
            }

            UpdatePlaylistMessage updatePlaylistMessage;
            if (MessageService.TryParseMessage(e.Data, out updatePlaylistMessage))
            {
                CreatePlaybackList(updatePlaylistMessage.Tracks, updatePlaylistMessage.ClearCurrentList);
                return;
            }

            RemoveTrackMessage removeTrackMessage;
            if (MessageService.TryParseMessage(e.Data, out removeTrackMessage))
            {
                var index = _playlist.Items.ToList().FindIndex(i => (string)i.Source.CustomProperties[BackgroundAudioConstants.TrackId] == removeTrackMessage.Id);
                _playlist.Items.RemoveAt(index);
                CreatePlaybackList(updatePlaylistMessage.Tracks, updatePlaylistMessage.ClearCurrentList);
                return;
            }
        }

        void Playlist_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            var item = args.NewItem;
            Debug.WriteLine("Playlist_CurrentItemChanged: " + (item == null ? "null" : GetTrackId(item).ToString()));

            _transportService.UpdateTransportControlsOnNewTrack(item);

            string currentTrackId = null;
            if (item != null)
                currentTrackId = item.Source.CustomProperties[BackgroundAudioConstants.TrackId] as string;

            if (_foregroundAppState == AppState.Active)
                MessageService.SendMessageToForeground(new TrackChangedMessage(currentTrackId));
            else
                ApplicationSettingsHelper.SaveSettingsValue(BackgroundAudioConstants.TrackId, currentTrackId == null ? null : currentTrackId.ToString());
        }

        private void TransportService_TransportControlsButtonPressed(object sender, SystemMediaTransportControlsButtonPressedEventArgs e)
        {
            switch (e.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    Debug.WriteLine("Play button pressed");

                    // When the background task has been suspended and the SMTC
                    // starts it again, wait for task to start. 
                    bool result = _backgroundTaskStarted.WaitOne(5000);
                    if (!result)
                        throw new Exception("Background Task didnt initialize in time");

                    StartPlayback();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    Debug.WriteLine("Pause button pressed");
                    try
                    {
                        BackgroundMediaPlayer.Current.Pause();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                    break;
                case SystemMediaTransportControlsButton.Next:
                    Debug.WriteLine("Next button pressed");
                    SkipToNext();
                    break;
                case SystemMediaTransportControlsButton.Previous:
                    Debug.WriteLine("Previous button pressed");
                    SkipToPrevious();
                    break;
            }
        }

        private void CreatePlaybackList(IEnumerable<TrackModel> tracks, bool clear)
        {
            if (_playlist == null || clear)
            {
                _playlist = new MediaPlaybackList();
                _playlist.AutoRepeatEnabled = false;
            }
            AddTracksToPlaylist(tracks);
            if (clear)
            {
                BackgroundMediaPlayer.Current.AutoPlay = false;
                BackgroundMediaPlayer.Current.Source = _playlist;
            }
            _playlist.CurrentItemChanged += Playlist_CurrentItemChanged;
        }

        private void AddTracksToPlaylist(IEnumerable<TrackModel> tracks)
        {
            foreach (var track in tracks)
            {
                var source = MediaSource.CreateFromUri(track.MediaUri);
                source.CustomProperties[BackgroundAudioConstants.TrackId] = track.Id;
                source.CustomProperties[BackgroundAudioConstants.TrackUri] = track.MediaUri;
                source.CustomProperties[BackgroundAudioConstants.Title] = track.Title;
                source.CustomProperties[BackgroundAudioConstants.AlbumArt] = track.AlbumArtUri;
                _playlist.Items.Add(new MediaPlaybackItem(source));
            }
        }

        private string GetCurrentTrackId()
        {
            if (_playlist == null)
                return null;

            return GetTrackId(_playlist.CurrentItem);
        }

        private string GetTrackId(MediaPlaybackItem item)
        {
            if (item == null)
                return null;

            return item.Source.CustomProperties[BackgroundAudioConstants.TrackId] as string;
        }
    }
}
