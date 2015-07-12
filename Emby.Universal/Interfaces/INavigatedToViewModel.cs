using Windows.UI.Xaml.Navigation;

namespace Emby.Universal.Interfaces
{
    public interface INavigatedToViewModel
    {
        void NavigatedTo(NavigationEventArgs e);
    }
}