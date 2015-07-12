using Windows.UI.Xaml.Navigation;

namespace Emby.Universal.Interfaces
{
    public interface INavigatedFromViewModel
    {
        void NavigatedFrom(NavigationEventArgs e);
    }
}