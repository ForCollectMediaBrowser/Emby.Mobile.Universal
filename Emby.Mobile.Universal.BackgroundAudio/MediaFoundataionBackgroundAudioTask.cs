using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Emby.Mobile.Universal.BackgroundAudio.Messages;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;

namespace Emby.Mobile.Universal.BackgroundAudio
{
    public sealed class MediaFoundataionBackgroundAudioTask : IBackgroundTask
    {
        private const string TrackIdKey = "trackid";
        private const string TrackUriKey = "trackuri";
        private const string TitleKey = "title";
        private const string AlbumArtKey = "albumart";

        private MediaPlaybackList _playlist = new MediaPlaybackList();
        private BackgroundTaskDeferral _deferral;
        private AppState _foregroundAppState = AppState.Unknown;
        private SystemMediaTransportControls _transportControls;
        private ManualResetEvent _backgroundTaskStarted = new ManualResetEvent(false);
        private bool _playbackStartedPreviously = false;

        #region Helper methods
        private string GetCurrentTrackId()
        {
            if (_playlist == null)
                return null;

            return GetTrackId(_playlist.CurrentItem);
        }

        private string GetTrackId(MediaPlaybackItem item)
        {
            if (item == null)
                return null; // no track playing

            return item.Source.CustomProperties[TrackIdKey] as string;
        }
        #endregion

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("Background Audio Task " + taskInstance.Task.Name + " starting...");

            ConfigureSystemMediaTransportControls();

            ReadStoredAppState();

            BackgroundMediaPlayer.Current.CurrentStateChanged += Current_CurrentStateChanged;
            BackgroundMediaPlayer.MessageReceivedFromForeground += BackgroundMediaPlayer_MessageReceivedFromForeground;

            if (_foregroundAppState != AppState.Suspended)
                MessageService.SendMessageToForeground(new BackgroundAudioTaskStartedMessage());

            ApplicationSettingsHelper.SaveSettingsValue(ApplicationSettingsConstants.BackgroundAudioTaskState, BackgroundTaskState.Running.ToString());

            _deferral = taskInstance.GetDeferral();
            _backgroundTaskStarted.Set();

            taskInstance.Task.Completed += TaskCompleted;
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
        }

        private void ConfigureSystemMediaTransportControls()
        {
            _transportControls = BackgroundMediaPlayer.Current.SystemMediaTransportControls;
            _transportControls.ButtonPressed += TransportControls_ButtonPressed;
            _transportControls.PropertyChanged += TransportControls_PropertyChanged;
            _transportControls.IsEnabled = true;
            _transportControls.IsPauseEnabled = true;
            _transportControls.IsPlayEnabled = true;
            _transportControls.IsNextEnabled = true;
            _transportControls.IsPreviousEnabled = true;
        }

        private void ReadStoredAppState()
        {
            var value = ApplicationSettingsHelper.ReadResetSettingsValue(ApplicationSettingsConstants.AppState);
            if (value == null)
                _foregroundAppState = AppState.Unknown;
            else
                _foregroundAppState = EnumHelper.Parse<AppState>(value.ToString());
        }

        private void TaskCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            Debug.WriteLine("MyBackgroundAudioTask " + sender.TaskId + " Completed...");
            _deferral.Complete();
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            Debug.WriteLine("MyBackgroundAudioTask " + sender.Task.TaskId + " Cancel Requested...");
            try
            {
                // immediately set not running
                _backgroundTaskStarted.Reset();

                // save state
                ApplicationSettingsHelper.SaveSettingsValue(ApplicationSettingsConstants.TrackId, GetCurrentTrackId() == null ? null : GetCurrentTrackId().ToString());
                ApplicationSettingsHelper.SaveSettingsValue(ApplicationSettingsConstants.Position, BackgroundMediaPlayer.Current.Position.ToString());
                ApplicationSettingsHelper.SaveSettingsValue(ApplicationSettingsConstants.BackgroundAudioTaskState, BackgroundTaskState.Canceled.ToString());
                ApplicationSettingsHelper.SaveSettingsValue(ApplicationSettingsConstants.AppState, Enum.GetName(typeof(AppState), _foregroundAppState));

                // unsubscribe from list changes
                if (_playlist != null)
                {
                    _playlist.CurrentItemChanged -= Playlist_CurrentItemChanged;
                    _playlist = null;
                }

                BackgroundMediaPlayer.MessageReceivedFromForeground -= BackgroundMediaPlayer_MessageReceivedFromForeground;
                _transportControls.ButtonPressed -= TransportControls_ButtonPressed;
                _transportControls.PropertyChanged -= TransportControls_PropertyChanged;

                BackgroundMediaPlayer.Shutdown();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            _deferral.Complete();
            Debug.WriteLine("MyBackgroundAudioTask Cancel complete...");
        }

        //Transport Controls
        private void TransportControls_PropertyChanged(SystemMediaTransportControls sender, SystemMediaTransportControlsPropertyChangedEventArgs args)
        {
            if (args.Property == SystemMediaTransportControlsProperty.SoundLevel)
            {
                if (sender.SoundLevel == SoundLevel.Muted && BackgroundMediaPlayer.Current.PlaybackRate > 0)
                {
                    BackgroundMediaPlayer.Current.Pause();
                }
                else if (sender.SoundLevel != SoundLevel.Muted && BackgroundMediaPlayer.Current.PlaybackRate == 0)
                {
                    BackgroundMediaPlayer.Current.Play();
                }
            }

        }

        private void TransportControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
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

        private void UpdateTransportControlsOnNewTrack(MediaPlaybackItem item)
        {
            if (item == null)
            {
                _transportControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
                _transportControls.DisplayUpdater.MusicProperties.Title = string.Empty;
                _transportControls.DisplayUpdater.Update();
                return;
            }

            _transportControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            _transportControls.DisplayUpdater.Type = MediaPlaybackType.Music;
            _transportControls.DisplayUpdater.MusicProperties.Title = item.Source.CustomProperties[TitleKey] as string;

            var albumArtUri = item.Source.CustomProperties[AlbumArtKey] as Uri;
            if (albumArtUri != null)
                _transportControls.DisplayUpdater.Thumbnail = RandomAccessStreamReference.CreateFromUri(albumArtUri);
            else
                _transportControls.DisplayUpdater.Thumbnail = null;

            _transportControls.DisplayUpdater.Update();
        }

        #region Playlist management functions and handlers
        
        private void StartPlayback()
        {
            try
            {
                // If playback was already started once we can just resume playing.
                if (!_playbackStartedPreviously)
                {
                    _playbackStartedPreviously = true;

                    // If the task was cancelled we would have saved the current track and its position. We will try playback from there.
                    var currentTrackId = ApplicationSettingsHelper.ReadResetSettingsValue(ApplicationSettingsConstants.TrackId);
                    var currentTrackPosition = ApplicationSettingsHelper.ReadResetSettingsValue(ApplicationSettingsConstants.Position);
                    if (currentTrackId != null)
                    {
                        // Find the index of the item by name
                        var index = _playlist.Items.ToList().FindIndex(item =>
                            GetTrackId(item).ToString() == (string)currentTrackId);

                        if (currentTrackPosition == null)
                        {
                            // Play from start if we dont have position
                            Debug.WriteLine("StartPlayback: Switching to track " + index);
                            _playlist.MoveTo((uint)index);

                            // Begin playing
                            BackgroundMediaPlayer.Current.Play();
                        }
                        else
                        {
                            // Play from exact position otherwise
                            TypedEventHandler<MediaPlaybackList, CurrentMediaPlaybackItemChangedEventArgs> handler = null;
                            handler = (MediaPlaybackList list, CurrentMediaPlaybackItemChangedEventArgs args) =>
                            {
                                if (args.NewItem == _playlist.Items[index])
                                {
                                    // Unsubscribe because this only had to run once for this item
                                    _playlist.CurrentItemChanged -= handler;

                                    // Set position
                                    var position = TimeSpan.Parse((string)currentTrackPosition);
                                    Debug.WriteLine("StartPlayback: Setting Position " + position);
                                    BackgroundMediaPlayer.Current.Position = position;

                                    // Begin playing
                                    BackgroundMediaPlayer.Current.Play();
                                }
                            };
                            _playlist.CurrentItemChanged += handler;

                            // Switch to the track which will trigger an item changed event
                            Debug.WriteLine("StartPlayback: Switching to track " + index);
                            _playlist.MoveTo((uint)index);
                        }
                    }
                    else
                    {
                        // Begin playing
                        BackgroundMediaPlayer.Current.Play();
                    }
                }
                else
                {
                    // Begin playing
                    BackgroundMediaPlayer.Current.Play();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }


        void Playlist_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            // Get the new item
            var item = args.NewItem;
            Debug.WriteLine("Playlist_CurrentItemChanged: " + (item == null ? "null" : GetTrackId(item).ToString()));

            // Update the system view
            UpdateTransportControlsOnNewTrack(item);

            // Get the current track
            string currentTrackId = null;
            if (item != null)
                currentTrackId = item.Source.CustomProperties[TrackIdKey] as string;

            // Notify foreground of change or persist for later
            if (_foregroundAppState == AppState.Active)
                MessageService.SendMessageToForeground(new TrackChangedMessage(currentTrackId));
            else
                ApplicationSettingsHelper.SaveSettingsValue(TrackIdKey, currentTrackId == null ? null : currentTrackId.ToString());
        }

        private void SkipToPrevious()
        {
            _transportControls.PlaybackStatus = MediaPlaybackStatus.Changing;
            _playlist.MovePrevious();
            BackgroundMediaPlayer.Current.Play();
        }

        private void SkipToNext()
        {
            _transportControls.PlaybackStatus = MediaPlaybackStatus.Changing;
            _playlist.MoveNext();
            BackgroundMediaPlayer.Current.Play();
        }
        #endregion

        #region Background Media Player Handlers
        private void Current_CurrentStateChanged(MediaPlayer sender, object args)
        {
            if (sender.CurrentState == MediaPlayerState.Playing)
            {
                _transportControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            }
            else if (sender.CurrentState == MediaPlayerState.Paused)
            {
                _transportControls.PlaybackStatus = MediaPlaybackStatus.Paused;
            }
            else if (sender.CurrentState == MediaPlayerState.Closed)
            {
                _transportControls.PlaybackStatus = MediaPlaybackStatus.Closed;
            }
        }
        private void BackgroundMediaPlayer_MessageReceivedFromForeground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            AppSuspendedMessage appSuspendedMessage;
            if (MessageService.TryParseMessage(e.Data, out appSuspendedMessage))
            {
                Debug.WriteLine("App suspending"); 
                _foregroundAppState = AppState.Suspended;
                var currentTrackId = GetCurrentTrackId();
                ApplicationSettingsHelper.SaveSettingsValue(ApplicationSettingsConstants.TrackId, currentTrackId == null ? null : currentTrackId.ToString());
                return;
            }

            AppResumedMessage appResumedMessage;
            if (MessageService.TryParseMessage(e.Data, out appResumedMessage))
            {
                Debug.WriteLine("App resuming");
                _foregroundAppState = AppState.Active;
                return;
            }

            StartPlaybackMessage startPlaybackMessage;
            if (MessageService.TryParseMessage(e.Data, out startPlaybackMessage))
            {
                Debug.WriteLine("Starting Playback");
                StartPlayback();
                return;
            }

            SkipNextMessage skipNextMessage;
            if (MessageService.TryParseMessage(e.Data, out skipNextMessage))
            {
                Debug.WriteLine("Skipping to next");
                SkipToNext();
                return;
            }

            SkipPreviousMessage skipPreviousMessage;
            if (MessageService.TryParseMessage(e.Data, out skipPreviousMessage))
            {
                Debug.WriteLine("Skipping to previous");
                SkipToPrevious();
                return;
            }

            TrackChangedMessage trackChangedMessage;
            if (MessageService.TryParseMessage(e.Data, out trackChangedMessage))
            {
                var index = _playlist.Items.ToList().FindIndex(i => (string)i.Source.CustomProperties[TrackIdKey] == trackChangedMessage.Id);
                Debug.WriteLine("Skipping to track " + index);
                _transportControls.PlaybackStatus = MediaPlaybackStatus.Changing;
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
        }

        void CreatePlaybackList(IEnumerable<TrackModel> tracks, bool clear)
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
                source.CustomProperties[TrackIdKey] = track.Id;
                source.CustomProperties[TrackUriKey] = track.MediaUri;
                source.CustomProperties[TitleKey] = track.Title;
                source.CustomProperties[AlbumArtKey] = track.AlbumArtUri;
                _playlist.Items.Add(new MediaPlaybackItem(source));
            }
        }

        #endregion
    }
}
