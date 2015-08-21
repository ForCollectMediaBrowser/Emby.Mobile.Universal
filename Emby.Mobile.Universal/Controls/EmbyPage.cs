using Cimbalino.Toolkit.Services;
using Emby.Mobile.Universal.Services;
using Emby.Mobile.Universal.ViewModel;

namespace Emby.Mobile.Universal.Controls
{
    public class EmbyPage : BasePage
    {
        public override INavigationService NavigationService { get; } = AppServices.NavigationService;
    }
}
