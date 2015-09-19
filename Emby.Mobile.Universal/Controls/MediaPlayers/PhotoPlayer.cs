using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Playback;
using Emby.Mobile.Universal.Extensions;
using Emby.Mobile.Universal.Services;
using Emby.Mobile.ViewModels.Entities;

namespace Emby.Mobile.Universal.Controls.MediaPlayers
{
    public class PhotoPlayer : Control, IMediaPlayer
    {
        public static readonly DependencyProperty PhotosProperty = DependencyProperty.Register(
            "Photos", typeof (List<PhotoViewModel>), typeof (PhotoPlayer), new PropertyMetadata(default(List<PlaylistItem>)));

        public List<PhotoViewModel> Photos
        {
            get { return (List<PhotoViewModel>) GetValue(PhotosProperty); }
            set { SetValue(PhotosProperty, value); }
        }

        public static readonly DependencyProperty SelectedPhotoProperty = DependencyProperty.Register(
            "SelectedPhoto", typeof (PhotoViewModel), typeof (PhotoPlayer), new PropertyMetadata(default(PlaylistItem)));

        public PhotoViewModel SelectedPhoto
        {
            get { return (PhotoViewModel) GetValue(SelectedPhotoProperty); }
            set { SetValue(SelectedPhotoProperty, value); }
        }

        public PhotoPlayer()
        {
            DefaultStyleKey = typeof (PhotoPlayer);
            AppServices.PlaybackService.RegisterPlayer(this);
        }

        public Guid Id { get; } = Guid.NewGuid();
        public PlayerType PlayerType { get; } = PlayerType.Image;
        public bool CanSeek { get; } = false;
        public bool CanPause { get; } = false;
        public bool IsPlaying => PlayerState == PlayerState.Playing;
        public PlayerState PlayerState { get; private set; }

        public Task Play(PlaylistItem item, long position = 0)
        {
            return Play(new List<PlaylistItem> { item }, position);
        }

        public Task Play(List<PlaylistItem> items, long position = 0, int? startingItem = null)
        {
            Photos = CreatePhotoList(items);
            SelectedPhoto = startingItem.HasValue ? Photos[startingItem.Value] : Photos.First();

            ShowPlayer();
            AppServices.NavigationService.NavigateToPhotoPlayer();

            PlayerState = PlayerState.Playing;

            return Task.FromResult(0);
        }

        private static List<PhotoViewModel> CreatePhotoList(List<PlaylistItem> items)
        {
            return items.Select(x => new PhotoViewModel(AppServices.Services, x.Item)).ToList();
        }

        public Task Add(List<PlaylistItem> items)
        {
            Photos.AddRange(CreatePhotoList(items));
            return Task.FromResult(0);
        }

        public Task Remove(PlaylistItem item)
        {
            var itemToRemove = Photos.FirstOrDefault(x => x.ItemInfo.Id == item.Item.Id);
            Photos.Remove(itemToRemove);
            return Task.FromResult(0);
        }

        public Task SkipToItem(PlaylistItem item)
        {
            SelectedPhoto = Photos.FirstOrDefault(x => x.ItemInfo.Id == item.Id.ToString());
            return Task.FromResult(0);
        }

        public void Stop()
        {
            PlayerState = PlayerState.Stopped;
            HidePlayer();
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
            if (Photos.Count < 2)
            {
                return Task.FromResult(false);
            }

            var currentIndex = Photos.IndexOf(SelectedPhoto);
            var nextIndex = 0;
            if (currentIndex < Photos.Count - 1)
            {
                nextIndex = currentIndex + 1;
            }

            SelectedPhoto = Photos[nextIndex];

            return Task.FromResult(true);
        }

        public Task<bool> SetPrevious()
        {
            if (Photos.Count < 2)
            {
                return Task.FromResult(false);
            }

            var currentIndex = Photos.IndexOf(SelectedPhoto);
            var nextIndex = 0;

            if (currentIndex == 0)
            {
                nextIndex = Photos.Count - 1;
            }
            else
            {
                nextIndex = currentIndex - 1;
            }

            SelectedPhoto = Photos[nextIndex];

            return Task.FromResult(true);
        }

        private void ShowPlayer()
        {
            if (Opacity < 1)
            {
                var sb = new Storyboard();
                sb.AddVisibleAnimation(this);
                sb.AddFadeAnim(this);
                sb.Begin();
                Width = double.NaN;
                Height = double.NaN;
                HorizontalAlignment = HorizontalAlignment.Stretch;
                VerticalAlignment = VerticalAlignment.Stretch;
            }
        }

        private void HidePlayer()
        {
            if (Opacity > 0)
            {
                this.Visibility = Visibility.Collapsed;
                var sb = new Storyboard();
                sb.AddFadeAnim(this, 0, 300);
                sb.AddWidthAnim(this, ActualWidth, 0, 300);
                sb.AddHeightAnim(this, ActualHeight, 0, 300);
                sb.AddCollapseAnimation(this, 300);
                sb.Begin();
            }
        }
    }
}