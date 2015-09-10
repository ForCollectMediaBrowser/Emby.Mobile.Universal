﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Emby.Mobile.Core.Playback;
using MediaBrowser.Model.Dto;
using Windows.UI.Xaml;
using Emby.Mobile.Universal.Services;
using MediaBrowser.Model.Session;
using MediaBrowser.Model.Dlna;
using GalaSoft.MvvmLight.Ioc;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.ApiInteraction.Playback;
using Emby.Mobile.Universal.Core.Helpers;
using Emby.Mobile.Core.Interfaces;
using Windows.Media.Playback;
using Emby.Mobile.Universal.BackgroundAudio.Messages;
using Emby.Mobile.Universal.BackgroundAudio;
using Emby.Mobile.Core.Helpers;
using Windows.ApplicationModel;
using System.Threading;
using Emby.Mobile.Core.Extensions;

namespace Emby.Mobile.Universal.Controls.MediaPlayers
{
    public class BackgroundAudioPlayer : IMediaPlayer
    {
        private DispatcherTimer _postionChangedTimer;
        private StreamInfo _streamInfo;
        private BaseItemDto _item;
        private MediaPlayer _player;
        private List<PlaylistItem> _playlist;
        private bool _isAudioBackgroundTaskRunning = false;
        private ManualResetEvent _backgroundTaskStarted = new ManualResetEvent(false);

        private bool IsAudioBackgroundTaskRunning
        {
            get
            {
                if (_isAudioBackgroundTaskRunning)
                    return true;

                string value = ApplicationSettingsHelper.ReadAndRemoveSettingsValue(BackgroundAudioConstants.BackgroundAudioTaskState) as string;
                if (value == null)
                {
                    return false;
                }
                else
                {
                    try
                    {
                        _isAudioBackgroundTaskRunning = value.ToEnum<BackgroundTaskState>() == BackgroundTaskState.Running;
                    }
                    catch (ArgumentException)
                    {
                        _isAudioBackgroundTaskRunning = false;
                    }
                    return _isAudioBackgroundTaskRunning;
                }
            }
        }

        public bool CanPause => _player?.CanPause == true;

        public bool CanSeek => _player?.CanSeek == true;

        public Guid Id { get; } = Guid.NewGuid();

        public bool IsPlaying => _player?.PlaybackRate > 0;

        public PlayerType PlayerType => PlayerType.Audio;

        #region Ctors and LifeCycle management

        public BackgroundAudioPlayer()
        {
            _postionChangedTimer = new DispatcherTimer();
            _postionChangedTimer.Interval = TimeSpan.FromSeconds(1);
            _postionChangedTimer.Tick += PostionChangedTimer_Tick;

            _player = BackgroundMediaPlayer.Current;
            _player.CurrentStateChanged += Player_CurrentStateChanged;
            _player.MediaOpened += Player_MediaOpened;
            _player.MediaEnded += Player_MediaEnded;
            _player.MediaFailed += Player_MediaFailed;

            AppServices.PlaybackService.RegisterPlayer(this);

            Application.Current.Suspending += ForegroundApp_Suspending;
            Application.Current.Resuming += ForegroundApp_Resuming;
            ApplicationSettingsHelper.SaveSettingsValue(BackgroundAudioConstants.AppState, AppState.Active.ToString());
        }

        private void ForegroundApp_Resuming(object sender, object e)
        {
            ApplicationSettingsHelper.SaveSettingsValue(BackgroundAudioConstants.AppState, AppState.Active.ToString());

            if (IsAudioBackgroundTaskRunning)
            {
                AddMediaPlayerEventHandlers();

                MessageService.SendMessageToBackground(new AppResumedMessage());
                var trackId = GetCurrentTrackIdAfterAppResume();
                var playlistItem = _playlist?.FirstOrDefault(t => t.Item.Id == trackId);
                if (playlistItem != null)
                {
                    _item = playlistItem.Item;
                }
            }
        }

        private void ForegroundApp_Suspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            if (IsAudioBackgroundTaskRunning)
            {
                BackgroundMediaPlayer.Current.CurrentStateChanged -= Player_CurrentStateChanged;
                BackgroundMediaPlayer.MessageReceivedFromBackground -= this.BackgroundMediaPlayer_MessageReceivedFromBackground; ;
                MessageService.SendMessageToBackground(new AppSuspendedMessage());
            }

            ApplicationSettingsHelper.SaveSettingsValue(BackgroundAudioConstants.AppState, AppState.Suspended.ToString());
            deferral.Complete();
        }

        private void AddMediaPlayerEventHandlers()
        {
            BackgroundMediaPlayer.Current.CurrentStateChanged += Player_CurrentStateChanged;
            BackgroundMediaPlayer.MessageReceivedFromBackground += BackgroundMediaPlayer_MessageReceivedFromBackground;
        }

        #endregion

        #region Player Eventhandlers

        private void Player_MediaFailed(MediaPlayer sender, object args)
        {
            AppServices.DispatcherService.RunAsync(() =>
            {
                _postionChangedTimer.Stop();
            });
        }

        private void Player_MediaEnded(MediaPlayer sender, object args)
        {
            AppServices.DispatcherService.RunAsync(() =>
            {
                _postionChangedTimer.Stop();
                AppServices.PlaybackService.ReportPlaybackStopped(new PlaybackStopInfo
                {
                    ItemId = _item.Id,
                    PositionTicks = _player.Position.Ticks
                }, _streamInfo);
            });
        }

        private void Player_MediaOpened(MediaPlayer sender, object args)
        {
            AppServices.DispatcherService.RunAsync(() =>
            {
                AppServices.PlaybackService.ReportPlaybackStarted(new PlaybackStartInfo
                {
                    ItemId = _item.Id,
                    CanSeek = CanSeek,
                    AudioStreamIndex = null,
                    IsPaused = _player.PlaybackRate == 0,
                    IsMuted = _player.IsMuted,
                    VolumeLevel = Convert.ToInt32(_player.Volume),
                    PositionTicks = _player.Position.Ticks
                });

                _postionChangedTimer.Start();
            });
        }

        private void Player_CurrentStateChanged(MediaPlayer sender, object args)
        {
            
        }

        #endregion

        private void PostionChangedTimer_Tick(object sender, object e)
        {
            AppServices.PlaybackService.ReportPlaybackProgress(new PlaybackProgressInfo
            {
                ItemId = _item.Id,
                CanSeek = CanSeek,
                AudioStreamIndex = null,
                IsPaused = _player.PlaybackRate == 0,
                IsMuted = _player.IsMuted,
                VolumeLevel = Convert.ToInt32(_player.Volume),
                PositionTicks = _player.Position.Ticks,
                PlayMethod = _streamInfo.PlayMethod
            }, _streamInfo);
        }

        async void BackgroundMediaPlayer_MessageReceivedFromBackground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            TrackChangedMessage trackChangedMessage;
            if (MessageService.TryParseMessage(e.Data, out trackChangedMessage))
            {
                await AppServices.DispatcherService.RunAsync(() =>
                {
                    if (trackChangedMessage.Id != null)
                    {
                        var playlistItem = _playlist?.FirstOrDefault(t => t.Item.Id == trackChangedMessage.Id);
                        if (playlistItem != null)
                        {
                            foreach(var track in _playlist.Where(p => p.State == PlaylistState.Playing))
                            {
                                track.State = PlaylistState.Played;
                            }
                            playlistItem.State = PlaylistState.Playing;
                            _item = playlistItem.Item;
                        }
                        AppServices.PlaybackService.ReportPlaylistStatus(_playlist);
                    }
                });
                return;
            }

            BackgroundAudioTaskStartedMessage backgroundAudioTaskStartedMessage;
            if (MessageService.TryParseMessage(e.Data, out backgroundAudioTaskStartedMessage))
            {
                await AppServices.DispatcherService.RunAsync(() =>
                {
                    AppServices.Log.Debug("BackgroundAudioTask started");
                    _backgroundTaskStarted.Set();
                });
                return;
            }
        }

        public void DecreaseVolume()
        {
            _player.Volume -= 1;
        }

        public void IncreaseVolume()
        {
            _player.Volume += 1;
        }

        public void Pause()
        {
            if (CanPause)
            {
                _player.Pause();
            }
        }

        public async Task Play(PlaylistItem item, double position = 0)
        {
            if (position > 0)
                ApplicationSettingsHelper.SaveSettingsValue(BackgroundAudioConstants.Position, TimeSpan.FromTicks((long)position));
            SendListToBackgroundPlayer(new List<TrackModel> { await GetTrackModel(item) }, true);
        }

        public async Task Play(List<PlaylistItem> items, double position = 0)
        {
            var list = new List<TrackModel>();
            foreach (var item in items)
            {
                list.Add(await GetTrackModel(item));
            }

            if (position > 0)
                ApplicationSettingsHelper.SaveSettingsValue(BackgroundAudioConstants.Position, TimeSpan.FromTicks((long)position));

            SendListToBackgroundPlayer(list, true);
        }

        public async Task Add(List<PlaylistItem> items)
        {
            var list = new List<TrackModel>();
            foreach (var item in items)
            {
                list.Add(await GetTrackModel(item));
            }

            SendListToBackgroundPlayer(list, false);
        }

        public Task Remove(PlaylistItem item)
        {
            if (_playlist.Contains(item))
            {
                _playlist.Remove(item);
                MessageService.SendMessageToBackground(new RemoveTrackMessage(item.Item.Id));
            }
            return Task.FromResult(0);
        }

        public Task SkipToItem(PlaylistItem item)
        {
            MessageService.SendMessageToBackground(new TrackChangedMessage(item.Item.Id));
            return Task.FromResult(0);
        }

        public Task<bool> Seek(long positionTicks)
        {
            if (CanSeek)
            {
                _player.Position = TimeSpan.FromTicks(positionTicks);
            }
            return Task.FromResult(true);
        }

        public Task<bool> SetNext()
        {
            MessageService.SendMessageToBackground(new SkipNextMessage());
            return Task.FromResult(true);
        }

        public Task<bool> SetPrevious()
        {
            MessageService.SendMessageToBackground(new SkipPreviousMessage());
            return Task.FromResult(true);
        }

        public void SetVolume(double value)
        {
            _player.Volume = value;
        }

        public void Stop() => _player.Pause();

        public void UnPause()
        {
            if (_player.PlaybackRate == 0)
            {
                _player.PlaybackRate = 1;
            }
        }

        private async Task<TrackModel> GetTrackModel(PlaylistItem item)
        {
            var client = SimpleIoc.Default.GetInstance<IConnectionManager>().GetApiClient(item.Item);
            var profile = await ConnectionManagerFactory.GetProfileAsync();
            _item = item.Item;
            _streamInfo = await item.GetStreamInfoAsync(0,
                                                   SimpleIoc.Default.GetInstance<IServerInfoService>().IsOffline,
                                                   SimpleIoc.Default.GetInstance<IPlaybackManager>(),
                                                   client,
                                                   profile);

            var url = _streamInfo.ToUrl(client.GetApiUrl("/"), client.AccessToken);
            var artUri = _item.HasPrimaryImage ? new Uri(client.GetImageUrl(_item, ImageOptionsHelper.ItemPrimary)) : null;
            return new TrackModel
            {
                Id = _item.Id,
                Title = _item.Name,
                AlbumArtUri = artUri,
                MediaUri = new Uri(url)
            };
        }

        private async void SendListToBackgroundPlayer(List<TrackModel> tracks, bool clearCurrentList)
        {
            if (IsAudioBackgroundTaskRunning || await StartBackgroundAudioTask())
            {
                MessageService.SendMessageToBackground(new UpdatePlaylistMessage(tracks, clearCurrentList));
                if (clearCurrentList)
                    MessageService.SendMessageToBackground(new StartPlaybackMessage());
            }
        }

        private async Task<bool> StartBackgroundAudioTask()
        {
            AddMediaPlayerEventHandlers();

            bool isSuccess = false;
            await AppServices.DispatcherService.RunAsync(() =>
            {
                isSuccess = _backgroundTaskStarted.WaitOne(10000);
            });
            return isSuccess;
        }

        private string GetCurrentTrackIdAfterAppResume()
        {
            object value = ApplicationSettingsHelper.ReadAndRemoveSettingsValue(BackgroundAudioConstants.TrackId);
            if (value != null)
                return (string)value;
            else
                return null;
        }

        //Not supported stuff
        public void NextAudioStream() { }
        public void NextSubtitleStream() { }
        public void SetAudioStreamIndex(int audioStreamIndex) { }
        public void SetSubtitleStreamIndex(int? subtitleStreamIndex) { }
    }
}
