using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Universal.Services;

namespace Emby.Mobile.Universal.Views.Players
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PhotoPlayerView
    {
        public PhotoPlayerView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            AppServices.PlaybackService.Stop();
            base.OnNavigatedTo(e);
        }
    }
}
