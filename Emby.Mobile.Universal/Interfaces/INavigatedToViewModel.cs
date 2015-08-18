using Windows.UI.Xaml.Navigation;

namespace Emby.Mobile.Universal.Interfaces
{
    public interface INavigatedToViewModel
    {
        void NavigatedTo(NavigationEventArgs e);
    }
}