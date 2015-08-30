using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Emby.Mobile.Universal.Controls
{
    public sealed class ProfilePicture : Control
    {
        public static readonly DependencyProperty ShowBackgroundProperty = DependencyProperty.Register(
            "ShowBackground", typeof (bool), typeof (ProfilePicture), new PropertyMetadata(default(bool)));

        public bool ShowBackground
        {
            get { return (bool) GetValue(ShowBackgroundProperty); }
            set { SetValue(ShowBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ProfileImageProperty = DependencyProperty.Register(
            "ProfileImage", typeof (string), typeof (ProfilePicture), new PropertyMetadata(default(string)));

        public string ProfileImage
        {
            get { return (string) GetValue(ProfileImageProperty); }
            set { SetValue(ProfileImageProperty, value); }
        }

        public ProfilePicture()
        {
            DefaultStyleKey = typeof(ProfilePicture);
        }

        private ImageBrush _imageBrush;
        private Grid _placeHolderGrid;
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _imageBrush = GetTemplateChild("ProfileImage") as ImageBrush;
            _placeHolderGrid = GetTemplateChild("PlaceholderGrid") as Grid;

            if (_imageBrush != null)
            {
                _imageBrush.ImageOpened -= ImageBrushOnImageOpened;
                _imageBrush.ImageOpened += ImageBrushOnImageOpened;

                _imageBrush.ImageFailed -= ImageBrushOnImageFailed;
                _imageBrush.ImageFailed += ImageBrushOnImageFailed;
            }
        }

        private void ImageBrushOnImageFailed(object sender, ExceptionRoutedEventArgs exceptionRoutedEventArgs)
        {
            var i = 12;
        }

        private void ImageBrushOnImageOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_placeHolderGrid != null)
            {
                _placeHolderGrid.Visibility = Visibility.Collapsed;
            }
        }
    }
}
