using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Universal.ViewModel;
using Emby.Mobile.ViewModels;
using Emby.Mobile.Universal.Interfaces;
using Emby.Mobile.Universal.Services;

namespace Emby.Mobile.Universal.Controls
{
    public class EmbyApplicationFrame : Frame
    {
        public void LoadLazyItems()
        {
            GetTemplateChild("VideoPlayer");
            GetTemplateChild("PhotoPlayer");
            var header = GetTemplateChild("Header") as HeaderControl;
            GetTemplateChild("StatusBar");

            if (header != null)
            {
                header.Margin = AppServices.DeviceInfo.SupportsStatusBar ? new Thickness(0, 20, 0, 0) : new Thickness(0);
            }

            Navigated += OnNavigated;
        }

        private static void OnNavigated(object sender, NavigationEventArgs e)
        {
            var hasHeader = e.Content as ICanHasHeaderMenu;
            var show = hasHeader != null;

            ViewModelLocator.Get<HeaderMenuViewModel>().ShowHide(show);
        }
    }
}
