using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Emby.Mobile.Universal.Controls
{
    [TemplatePart(Name = TheImage, Type = typeof(Image))]
    [TemplatePart(Name = PlaceHolder, Type = typeof(ContentPresenter))]
    [TemplateVisualState(GroupName = ImageLoadedGroup, Name = LoadedState)]
    [TemplateVisualState(GroupName = ImageLoadedGroup, Name = NotLoadedState)]
    public sealed class EmbyImage : Control
    {
        private const string ImageLoadedGroup = "ImageLoadedGroup";
        private const string LoadedState = "Loaded";
        private const string NotLoadedState = "NotLoaded";
        private const string TheImage = "TheImage";
        private const string PlaceHolder = "PlaceholderContent";

        // This may change to something else, but for now, just use an Image control in the template
        private Image _image;
        private ContentPresenter _placeholder;

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch", typeof(Stretch), typeof(EmbyImage), new PropertyMetadata(default(Stretch)));

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(ImageSource), typeof(EmbyImage), new PropertyMetadata(default(ImageSource)));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
            "Placeholder", typeof(object), typeof(EmbyImage), new PropertyMetadata(default(object)));

        public object Placeholder
        {
            get { return GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTypeProperty = DependencyProperty.Register(
            "PlaceholderType", typeof(PlaceholderType), typeof(EmbyImage), new PropertyMetadata(default(PlaceholderType), OnPlaceholderTypeChanged));

        public PlaceholderType PlaceholderType
        {
            get { return (PlaceholderType)GetValue(PlaceholderTypeProperty); }
            set { SetValue(PlaceholderTypeProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderStretchProperty = DependencyProperty.Register(
            "PlaceholderStretch", typeof(Stretch), typeof(EmbyImage), new PropertyMetadata(default(Stretch)));

        public Stretch PlaceholderStretch
        {
            get { return (Stretch)GetValue(PlaceholderStretchProperty); }
            set { SetValue(PlaceholderStretchProperty, value); }
        }

        public static readonly DependencyProperty ImageTemplateProperty = DependencyProperty.Register(
            "ImageTemplate", typeof(DataTemplate), typeof(EmbyImage), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ImageTemplate
        {
            get { return (DataTemplate)GetValue(ImageTemplateProperty); }
            set { SetValue(ImageTemplateProperty, value); }
        }

        public static readonly DependencyProperty FontTemplateProperty = DependencyProperty.Register(
            "FontTemplate", typeof(DataTemplate), typeof(EmbyImage), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate FontTemplate
        {
            get { return (DataTemplate)GetValue(FontTemplateProperty); }
            set { SetValue(FontTemplateProperty, value); }
        }

        public static readonly DependencyProperty SymbolTemplateProperty = DependencyProperty.Register(
            "SymbolTemplate", typeof(DataTemplate), typeof(EmbyImage), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate SymbolTemplate
        {
            get { return (DataTemplate)GetValue(SymbolTemplateProperty); }
            set { SetValue(SymbolTemplateProperty, value); }
        }

        public EmbyImage()
        {
            DefaultStyleKey = typeof(EmbyImage);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _placeholder = GetTemplateChild(PlaceHolder) as ContentPresenter;
            _image = GetTemplateChild(TheImage) as Image;
            if (_image != null)
            {
                _image.ImageOpened -= ImageOnImageOpened;
                _image.ImageOpened += ImageOnImageOpened;

                _image.ImageFailed -= ImageOnImageFailed;
                _image.ImageFailed += ImageOnImageFailed;
            }

            SetPlaceholder();
        }

        private static void OnPlaceholderTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as EmbyImage)?.SetPlaceholder();
        }

        private void SetPlaceholder()
        {
            if (_placeholder == null)
            {
                return;
            }

            switch (PlaceholderType)
            {
                case PlaceholderType.Font:
                    _placeholder.ContentTemplate = FontTemplate;
                    break;
                case PlaceholderType.Image:
                    _placeholder.ContentTemplate = ImageTemplate;
                    break;
                case PlaceholderType.Symbol:
                    _placeholder.ContentTemplate = SymbolTemplate;
                    break;
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

    public enum PlaceholderType
    {
        Image,
        Font,
        Symbol
    }
}
