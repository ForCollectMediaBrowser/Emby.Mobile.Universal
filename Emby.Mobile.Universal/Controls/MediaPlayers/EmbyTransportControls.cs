using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Controls.MediaPlayers
{
    public class EmbyTransportControls : MediaTransportControls
    {
        public static readonly DependencyProperty VideoTitleProperty = DependencyProperty.Register(
            "VideoTitle", typeof (string), typeof (EmbyTransportControls), new PropertyMetadata(default(string)));

        public string VideoTitle
        {
            get { return (string) GetValue(VideoTitleProperty); }
            set { SetValue(VideoTitleProperty, value); }
        }

        public static readonly DependencyProperty LogoImageUrlProperty = DependencyProperty.Register(
            "LogoImageUrl", typeof (string), typeof (EmbyTransportControls), new PropertyMetadata(default(string)));

        public string LogoImageUrl
        {
            get { return (string) GetValue(LogoImageUrlProperty); }
            set { SetValue(LogoImageUrlProperty, value); }
        }

        public EmbyTransportControls()
        {
            DefaultStyleKey = typeof (EmbyTransportControls);
        }
    }
}
