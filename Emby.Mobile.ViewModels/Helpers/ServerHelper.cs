using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Strings;

namespace Emby.Mobile.Helpers
{
    public static class ServerHelper
    {
        public static async Task ChangeServer(IServices services)
        {
            var isConnectUser = services.Authentication.SignedInUsingConnect;
            if (isConnectUser)
            {
                services.UiInteractions.NavigationService.NavigateToServerSelection();
                services.UiInteractions.NavigationService.ClearBackStack();
            }
            else
            {
                if (services.Authentication.IsSignedIn)
                {
                    var result = await services.UiInteractions.MessageBox.ShowAsync(Resources.MessageSignOutOfCurrentUser, Resources.MessageAreYouSureTitle, new[] {Resources.LabelYes, Resources.LabelNo});
                    if (result == 0)
                    {
                        if (await services.Authentication.SignOut(true))
                        {
                            services.UiInteractions.NavigationService.NavigateToManualServerEntry();
                            services.UiInteractions.NavigationService.ClearBackStack();
                        }
                    }
                }
                else
                {
                    services.ServerInfo.Clear();
                    services.UiInteractions.NavigationService.NavigateToManualServerEntry();
                    services.UiInteractions.NavigationService.ClearBackStack();
                }
            }
        }
    }
}
