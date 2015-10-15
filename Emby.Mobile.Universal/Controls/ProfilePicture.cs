using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Emby.Mobile.Universal.Controls
{
    [TemplateVisualState(GroupName = ImageLoadedGroup, Name = LoadedState)]
    [TemplateVisualState(GroupName = ImageLoadedGroup, Name = NotLoadedState)]
    public sealed class ProfilePicture : Control
    {
        private const string ImageLoadedGroup = "ImageLoadedGroup";
        private const string LoadedState = "Loaded";
        private const string NotLoadedState = "NotLoaded";

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

        public static readonly DependencyProperty CircleBackgroundProperty = DependencyProperty.Register(
            "CircleBackground", typeof (SolidColorBrush), typeof (ProfilePicture), new PropertyMetadata(default(SolidColorBrush)));

        public SolidColorBrush CircleBackground
        {
            get { return (SolidColorBrush) GetValue(CircleBackgroundProperty); }
            set { SetValue(CircleBackgroundProperty, value); }
        }

        public ProfilePicture()
        {
            DefaultStyleKey = typeof(ProfilePicture);
        }

        private ImageBrush _imageBrush;
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _imageBrush = GetTemplateChild("ProfileImage") as ImageBrush;

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
            VisualStateManager.GoToState(this, NotLoadedState, true);
        }

        private void ImageBrushOnImageOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            VisualStateManager.GoToState(this, LoadedState, true);
        }
    }
}
