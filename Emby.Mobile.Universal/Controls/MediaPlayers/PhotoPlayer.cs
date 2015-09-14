using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Playback;
using Emby.Mobile.Universal.Services;

namespace Emby.Mobile.Universal.Controls.MediaPlayers
{
    public class PhotoPlayer : Control, IMediaPlayer
    {
        public static readonly DependencyProperty PhotosProperty = DependencyProperty.Register(
            "Photos", typeof (List<PlaylistItem>), typeof (PhotoPlayer), new PropertyMetadata(default(List<PlaylistItem>)));

        public List<PlaylistItem> Photos
        {
            get { return (List<PlaylistItem>) GetValue(PhotosProperty); }
            set { SetValue(PhotosProperty, value); }
        }

        public static readonly DependencyProperty SelectedPhotoProperty = DependencyProperty.Register(
            "SelectedPhoto", typeof (PlaylistItem), typeof (PhotoPlayer), new PropertyMetadata(default(PlaylistItem)));

        public PlaylistItem SelectedPhoto
        {
            get { return (PlaylistItem) GetValue(SelectedPhotoProperty); }
            set { SetValue(SelectedPhotoProperty, value); }
        }

        public PhotoPlayer()
        {
            DefaultStyleKey = typeof (PhotoPlayer);
        }

        public Guid Id { get; } = Guid.NewGuid();
        public PlayerType PlayerType { get; } = PlayerType.Image;
        public bool CanSeek { get; } = false;
        public bool CanPause { get; } = false;
        public bool IsPlaying { get; }
        public PlayerState PlayerState { get; }
        public Task Play(PlaylistItem item, double position = 0)
        {
            return Task.FromResult(0);
        }

        public Task Play(List<PlaylistItem> items, double position = 0)
        {
            Photos = items;
            return Task.FromResult(0);
        }

        public Task Add(List<PlaylistItem> items)
        {
            return Task.FromResult(0);
        }

        public Task Remove(PlaylistItem item)
        {
            return Task.FromResult(0);
        }

        public Task SkipToItem(PlaylistItem item)
        {
            return Task.FromResult(0);
        }

        public void Stop()
        {
        }

        public void Pause()
        {
        }

        public void UnPause()
        {
        }

        public Task<bool> Seek(long positionTicks)
        {
            return Task.FromResult(false);
        }

        public void SetSubtitleStreamIndex(int? subtitleStreamIndex)
        {
        }

        public void NextSubtitleStream()
        {
        }

        public void SetAudioStreamIndex(int audioStreamIndex)
        {
        }

        public void NextAudioStream()
        {
        }

        public void IncreaseVolume()
        {
        }

        public void DecreaseVolume()
        {
        }

        public void SetVolume(double value)
        {
        }

        public Task<bool> SetNext()
        {
            return Task.FromResult(false);
        }

        public Task<bool> SetPrevious()
        {
            return Task.FromResult(false);
        }

        protected override void OnApplyTemplate()
        {
            AppServices.PlaybackService.RegisterPlayer(this);
            base.OnApplyTemplate();
        }
    }
}