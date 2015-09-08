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

namespace Emby.Mobile.Universal.Controls.MediaPlayers
{
    [TemplatePart(Name = "Player", Type = typeof(MediaElement))]
    public class MediaFoundationAudioPlayer : Control, IMediaPlayer
    {
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
            DefaultStyleKey = typeof(MediaFoundationAudioPlayer);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _player = GetTemplateChild("Player") as MediaElement;
            _player.CurrentStateChanged += Player_CurrentStateChanged;
            _player.VolumeChanged += Player_VolumeChanged;
            _player.MediaOpened += Player_MediaOpened;
            _player.MediaEnded += Player_MediaEnded;

            AppServices.PlaybackService.RegisterPlayer(this);
        }

        private void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
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
            throw new NotImplementedException();
        }

        public Task Play(List<PlaylistItem> items, double position = 0)
        {
            throw new NotImplementedException();
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
    }
}
