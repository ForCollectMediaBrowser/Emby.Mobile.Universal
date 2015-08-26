using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Universal.Core.Helpers
{
    public static class SignOutHelper
    {
        public static async Task SignOut(IServices services)
        {
            var signedInWithConnect = services.Authentication.SignedInUsingConnect;
            if (await services.Authentication.SignOut())
            {
                if (signedInWithConnect)
                {
                    services.NavigationService.NavigateToEmbyConnect();
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
