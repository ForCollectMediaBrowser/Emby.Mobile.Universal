using Cimbalino.Toolkit.Services;
using Emby.Mobile.Universal.Controls;
using Emby.Mobile.Universal.Services;

namespace Emby.Mobile.Universal.Views
{
    public class BaseView : BasePage
    {
        public override INavigationService NavigationService => AppServices.NavigationService;
    }
}
