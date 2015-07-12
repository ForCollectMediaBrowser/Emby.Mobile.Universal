using Cimbalino.Toolkit.Services;
using Emby.Universal.ViewModel;

namespace Emby.Universal.Controls
{
    public class EmbyPage : BasePage
    {
        public override INavigationService NavigationService { get; } = ViewModelLocator.NavigationService;
    }
}
