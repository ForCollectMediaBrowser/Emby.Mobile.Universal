using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Universal.ViewModel;
using Emby.Mobile.ViewModels;
using Emby.Mobile.Universal.Interfaces;

namespace Emby.Mobile.Universal.Controls
{
    public class EmbyApplicationFrame : Frame
    {        

        public EmbyApplicationFrame()
        {
            Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            var hasHeader = e.Content as ICanHasHeaderMenu;
            var show = hasHeader != null;

            ViewModelLocator.Get<HeaderMenuViewModel>().ShowHide(show);
        }
    }
}
