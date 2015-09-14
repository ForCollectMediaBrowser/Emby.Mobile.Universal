using System;
using Emby.Mobile.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Emby.Mobile.Universal.Controls
{
    public sealed partial class FadingImages : UserControl, IDisposable
    {
        private const string FirstState = "First";
        private const string SecondState = "Second";
        public static readonly DependencyProperty ImageSourcesProperty = DependencyProperty.Register("ImageSources", typeof(string[]), typeof(FadingImages), new PropertyMetadata(new string[0], ImageSourcesSet));

        private readonly DispatcherTimer _updateTimer;
        private readonly BitmapImage _imageSource1;
        private readonly BitmapImage _imageSource2;
        private readonly ImageBrush _imageBrush1;
        private readonly ImageBrush _imageBrush2;

        private int _index = -1;

        public string[] ImageSources
        {
            get { return (string[])GetValue(ImageSourcesProperty); }
            set { SetValue(ImageSourcesProperty, value); }
        }

        public FadingImages()
        {
            _updateTimer = new DispatcherTimer();

            _updateTimer.Tick += (s, e) =>
            {
                ToggleState();
            };
            _updateTimer.Interval = TimeSpan.FromSeconds(10);

            _imageSource1 = new BitmapImage();
            _imageSource2 = new BitmapImage();
            _imageBrush1 = new ImageBrush
            {
                ImageSource = _imageSource1,
                Stretch = Stretch.UniformToFill,
                Opacity = 0.6d
            };

            _imageBrush2 = new ImageBrush
            {
                ImageSource = _imageSource2,
                Stretch = Stretch.UniformToFill,
                Opacity = 0.6d
            };
            InitializeComponent();

            ImageGrid1.Background = _imageBrush1;
            ImageGrid2.Background = _imageBrush2;
        }

        private static void ImageSourcesSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                var newValue = e.NewValue as string[];
                var oldValue = e.OldValue as string[];
                if (!AreValuesEqual(newValue, oldValue))
                {
                    var control = d as FadingImages;
                    control?.Reset();
                }
            }
        }

        private void Reset()
        {
            if (_updateTimer.IsEnabled)
            {
                _updateTimer.Stop();
            }
            if (ImageSources != null && ImageSources.Length > 0)
            {
                _index = -1;
                ToggleState();
                if (ImageSources.Length > 1)
                {
                    _updateTimer.Start();
                }
            }
        }

        private void ToggleState()
        {
            SetupNextImage();
            var state = VisualStateGroup.CurrentState;

            if (state == null || state.Name == SecondState)
            {
                VisualStateManager.GoToState(this, FirstState, true);
            }
            else
            {
                VisualStateManager.GoToState(this, SecondState, true);
            }
        }

        private void SetupNextImage()
        {
            var source = GetImageSource();
            if (ImageSources == null || ImageSources.Length == 0)
            {
                source.UriSource = null;
            }
            else
            {
                _index++;
                if (_index > ImageSources.Length - 1)
                {
                    _index = 0;
                }

                source.UriSource = new Uri(ImageSources[_index]);
            }
        }

        private static bool AreValuesEqual(string[] newValue, string[] oldValue)
        {
            if (oldValue == null || newValue == null)
            {
                return false;
            }

            if (newValue.Length != oldValue.Length)
            {
                return false;
            }

            for (int i = 0; i < newValue.Length; i++)
            {
                if (string.Compare(newValue[i], oldValue[i]) != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private BitmapImage GetImageSource()
        {
            var state = VisualStateGroup.CurrentState;

            if (state == null || state.Name == "Second")
            {
                return _imageSource1;
            }
            return _imageSource2;
        }

        public void Dispose()
        {
            _updateTimer.Stop();
        }
    }
}
