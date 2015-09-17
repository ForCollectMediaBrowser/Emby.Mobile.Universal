using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Universal.ViewModel;
using Emby.Mobile.ViewModels;
using Emby.Mobile.Universal.Interfaces;

namespace Emby.Mobile.Universal.Controls
{
    public class EmbyApplicationFrame : Frame
    {
        public void LoadLazyItems()
        {
            GetTemplateChild("VideoPlayer");
            GetTemplateChild("PhotoPlayer");
            GetTemplateChild("Header");
            GetTemplateChild("StatusBar");

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
