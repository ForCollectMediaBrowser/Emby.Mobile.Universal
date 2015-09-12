using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Helpers
{
    public static class SignOutHelper
    {
        public static async Task SignOut(IServices services)
        {
            var signedInWithConnect = services.Authentication.SignedInUsingConnect;
            if (await services.Authentication.SignOut(removeServerInfo: signedInWithConnect))
            {
                if (signedInWithConnect)
                {
                    services.ServerInfo.Clear();
                    if (ConnectHelper.UsePinLogin(services.Device.DeviceFamily))
                    {
                        services.UiInteractions.NavigationService.NavigateToPinLogin();
                    }
                    else
                    {
                        services.UiInteractions.NavigationService.NavigateToEmbyConnect();
                    }
                }
                else
                {
                    services.UiInteractions.NavigationService.NavigateToChooseProfile();
                }

                services.UiInteractions.NavigationService.ClearBackStack();
            }
        }
    }
}
