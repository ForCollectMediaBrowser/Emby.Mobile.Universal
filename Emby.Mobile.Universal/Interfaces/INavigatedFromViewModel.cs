using Windows.UI.Xaml.Navigation;

namespace Emby.Mobile.Universal.Interfaces
{
    public interface INavigatedFromViewModel
    {
        void NavigatedFrom(NavigationEventArgs e);
    }
}