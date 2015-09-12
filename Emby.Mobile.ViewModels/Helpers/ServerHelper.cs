using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Strings;

namespace Emby.Mobile.Helpers
{
    public static class ServerHelper
    {
        public static async Task ChangeServer(IServices services)
        {
            var isConnectUser = services.ServerInteractions.Authentication.SignedInUsingConnect;
            if (isConnectUser)
            {
                services.UiInteractions.NavigationService.NavigateToServerSelection();
                services.UiInteractions.NavigationService.ClearBackStack();
            }
            else
            {
                if (services.ServerInteractions.Authentication.IsSignedIn)
                {
                    var result = await services.UiInteractions.MessageBox.ShowAsync(Resources.MessageSignOutOfCurrentUser, Resources.MessageAreYouSureTitle, new[] {Resources.LabelYes, Resources.LabelNo});
                    if (result == 0)
                    {
                        if (await services.ServerInteractions.Authentication.SignOut(true))
                        {
                            services.UiInteractions.NavigationService.NavigateToManualServerEntry();
                            services.UiInteractions.NavigationService.ClearBackStack();
                        }
                    }
                }
                else
                {
                    services.ServerInteractions.ServerInfo.Clear();
                    services.UiInteractions.NavigationService.NavigateToManualServerEntry();
                    services.UiInteractions.NavigationService.ClearBackStack();
                }
            }
        }
    }
}
