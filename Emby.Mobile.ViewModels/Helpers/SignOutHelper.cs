using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Helpers;

namespace Emby.Mobile.Universal.Core.Helpers
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
                        services.NavigationService.NavigateToPinLogin();
                    }
                    else
                    {
                        services.NavigationService.NavigateToEmbyConnect();
                    }
                }
                else
                {
                    services.NavigationService.NavigateToChooseProfile();
                }

                services.NavigationService.ClearBackStack();
            }
        }
    }
}
