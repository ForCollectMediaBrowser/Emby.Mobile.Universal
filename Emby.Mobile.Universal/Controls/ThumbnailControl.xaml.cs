using Windows.UI.Xaml;
using Emby.Mobile.ViewModels.Entities;

namespace Emby.Mobile.Universal.Controls
{
    public sealed partial class ThumbnailControl
    {
        private ItemViewModel Item => DataContext as ItemViewModel;

        public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register(
            "ImageUrl", typeof (string), typeof (ThumbnailControl), new PropertyMetadata(" "));

        public string ImageUrl
        {
            get { return (string) GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        public ThumbnailControl()
        {
            InitializeComponent();
            DataContextChanged += (sender, args) =>
            {
                Bindings.Update();
            };
        }
    }
}
