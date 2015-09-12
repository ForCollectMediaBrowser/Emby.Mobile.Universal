using Emby.Mobile.Universal.Services;
using Windows.UI.Xaml.Navigation;

namespace Emby.Mobile.Universal.Views
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
