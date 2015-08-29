using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Services;

namespace Emby.Mobile.Universal.Controls
{
    public class EmbyPage : BasePage
    {
        public override INavigationService NavigationService { get; } = AppServices.NavigationService;
    }
}
