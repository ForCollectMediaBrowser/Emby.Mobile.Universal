using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Universal.Services;

namespace Emby.Mobile.Universal.Views.Players
{
    public sealed partial class MediaFoundationVideoPlaybackView
    {
        public MediaFoundationVideoPlaybackView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            AppServices.PlaybackService.Stop();
            base.OnNavigatingFrom(e);
        }
    }
}
