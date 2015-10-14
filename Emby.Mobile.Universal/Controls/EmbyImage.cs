using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Emby.Mobile.Universal.Controls
{
    [TemplatePart(Name = TheImage)]
    [TemplateVisualState(GroupName = ImageLoadedGroup, Name = LoadedState)]
    [TemplateVisualState(GroupName = ImageLoadedGroup, Name = NotLoadedState)]
    public sealed class EmbyImage : Control
    {
        private const string ImageLoadedGroup = "ImageLoadedGroup";
        private const string LoadedState = "Loaded";
        private const string NotLoadedState = "NotLoaded";
        private const string TheImage = "TheImage";

        // This may change to something else, but for now, just use an Image control in the template
        private Image _image;

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch", typeof (Stretch), typeof (EmbyImage), new PropertyMetadata(default(Stretch)));

        public Stretch Stretch
        {
            get { return (Stretch) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof (ImageSource), typeof (EmbyImage), new PropertyMetadata(default(ImageSource)));

        public ImageSource Source
        {
            get { return (ImageSource) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public EmbyImage()
        {
            DefaultStyleKey = typeof(EmbyImage);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _image = GetTemplateChild(TheImage) as Image;
            if (_image != null)
            {
                _image.ImageOpened -= ImageOnImageOpened;
                _image.ImageOpened += ImageOnImageOpened;

                _image.ImageFailed -= ImageOnImageFailed;
                _image.ImageFailed += ImageOnImageFailed;
            }
        }

        private void ImageOnImageFailed(object sender, ExceptionRoutedEventArgs exceptionRoutedEventArgs)
        {
            VisualStateManager.GoToState(this, NotLoadedState, true);
        }

        private void ImageOnImageOpened(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, LoadedState, true);
        }
    }
}
