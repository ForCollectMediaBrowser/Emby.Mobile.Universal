using Emby.Mobile.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Emby.Mobile.Core.Playback;
using MediaBrowser.Model.Dto;
using Windows.UI.Xaml;
using Emby.Mobile.Universal.Services;
using MediaBrowser.Model.Session;
using MediaBrowser.Model.Dlna;
using Emby.Mobile.Universal.Extensions;
using GalaSoft.MvvmLight.Ioc;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.ApiInteraction.Playback;
using MediaBrowser.Model.MediaInfo;
using MediaBrowser.ApiInteraction.Data;
using System.IO;
using MediaBrowser.Model.Net;
using Emby.Mobile.Universal.Core.Helpers;

namespace Emby.Mobile.Universal.Controls.MediaPlayers
{
    [TemplatePart(Name = "Player", Type = typeof(MediaElement))]
    public class MediaFoundationAudioPlayer : Control, IMediaPlayer
    {
        private DispatcherTimer _postionChangedTimer;
        private StreamInfo _streamInfo;
        private BaseItemDto _item;
        private MediaElement _player;
        public bool CanPause => _player?.CanPause == true;

        public bool CanSeek => _player?.CanSeek == true;

        public Guid Id { get; } = Guid.NewGuid();

        public bool IsPlaying => _player?.PlaybackRate > 0;

        public PlayerType PlayerType => PlayerType.Audio;

        public MediaFoundationAudioPlayer()
        {
            _postionChangedTimer = new DispatcherTimer();
            _postionChangedTimer.Interval = TimeSpan.FromSeconds(1);
            _postionChangedTimer.Tick += PostionChangedTimer_Tick;
            DefaultStyleKey = typeof(MediaFoundationAudioPlayer);
        }

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

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _player = GetTemplateChild("Player") as MediaElement;
            _player.CurrentStateChanged += Player_CurrentStateChanged;
            _player.VolumeChanged += Player_VolumeChanged;
            _player.MediaOpened += Player_MediaOpened;
            _player.MediaEnded += Player_MediaEnded;
            _player.MediaFailed += Player_MediaFailed;

            AppServices.PlaybackService.RegisterPlayer(this);
        }

        private void Player_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            _postionChangedTimer.Stop();
        }

        private void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            _postionChangedTimer.Stop();
            AppServices.PlaybackService.ReportPlaybackStopped(new PlaybackStopInfo
            {
                ItemId = _item.Id,
                PositionTicks = _player.Position.Ticks
            }, _streamInfo);
        }

        private void Player_MediaOpened(object sender, RoutedEventArgs e)
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
        }

        private void Player_VolumeChanged(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Player_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
          
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

        public Task Play(PlaylistItem item, double position = 0)
        {
            return PlayItem(item, (int)position);
        }

        public Task Play(List<PlaylistItem> items, double position = 0)
        {
            return PlayItem(items[0], (int)position);
        }

        public Task Add(List<PlaylistItem> items)
        {
            throw new NotImplementedException();
        }

        public Task Remove(PlaylistItem item)
        {
            throw new NotImplementedException();
        }

        public Task SkipToItem(PlaylistItem item)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public Task<bool> SetPrevious()
        {
            throw new NotImplementedException();
        }

        public void SetVolume(double value)
        {
            _player.Volume = value;
        }

        public void Stop()
        {
            _player.Stop();
        }

        public void UnPause()
        {
            if (_player.PlaybackRate == 0)
            {
                _player.Pause();
            }
        }

        //Not supported stuff
        public void NextAudioStream() { }
        public void NextSubtitleStream() { }
        public void SetAudioStreamIndex(int audioStreamIndex) { }
        public void SetSubtitleStreamIndex(int? subtitleStreamIndex) { }


        private async Task PlayItem(PlaylistItem item, int positionTicks)
        {
            var client = SimpleIoc.Default.GetInstance<IConnectionManager>().GetApiClient(item.Item);
            var profile = await ConnectionManagerFactory.GetProfileAsync();
            _item = item.Item;
            _streamInfo = await item.GetStreamInfoAsync(positionTicks,
                                                   SimpleIoc.Default.GetInstance<IServerInfoService>().IsOffline,
                                                   SimpleIoc.Default.GetInstance<IPlaybackManager>(),
                                                   client,
                                                   profile);

            if (_streamInfo.PlayMethod == PlayMethod.DirectPlay && _streamInfo.MediaSource?.Protocol == MediaProtocol.File)
            {
                await SetLocalFileSourceAsync();
            }
            else
            {
                SetRemoteSource(client);
                _player.Play();
            }
        }

        private async Task SetLocalFileSourceAsync()
        {
            var assetManager = SimpleIoc.Default.GetInstance<ILocalAssetManager>();
            try
            {
                var stream = await assetManager.GetFileStream(_streamInfo);
                _player.SetSource(stream.AsRandomAccessStream(), MimeTypes.GetMimeType("." + _streamInfo.Container));
            }
            catch (Exception e)
            {
                //_log.Error("Failed to set local file Source", e);
            }
        }

        private void SetRemoteSource(IApiClient client)
        {
            var url = _streamInfo.ToUrl(client.GetApiUrl("/"), client.AccessToken);
            var source = new Uri(url);
            _player.Source = source;
        }
    }
}
