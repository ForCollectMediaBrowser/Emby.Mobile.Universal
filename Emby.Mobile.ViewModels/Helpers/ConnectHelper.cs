using System.Threading.Tasks;
using Emby.Mobile.Core.Helpers;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Helpers
{
    public static class ConnectHelper
    {
        public async static Task HandleConnectState(ConnectionResult result, IServices services, IApiClient apiClient)
        {
            switch (result.State)
            {
                case ConnectionState.Unavailable:
                    await services.UiInteractions.MessageBox.ShowAsync("Could not find server");
                    services.UiInteractions.NavigationService.NavigateToConnectFirstRun();
                    break;
                case ConnectionState.ServerSelection:
                    services.UiInteractions.NavigationService.NavigateToServerSelection();
                    break;
                case ConnectionState.ServerSignIn:
                    if (services.Authentication.SignedInUser == null)
                    {                        
                        services.UiInteractions.NavigationService.NavigateToChooseProfile();
                    }
                    else
                    {
                        services.Authentication.SetAuthenticationInfo();
                        await services.StartUp.Startup();

                        services.UiInteractions.NavigationService.NavigateToHome();
                    }
                    break;
                case ConnectionState.SignedIn:
                    if (services.Authentication.SignedInUser == null)
                    {
                        var user = await apiClient.GetUserAsync(apiClient.CurrentUserId);
                        services.Authentication.SetUser(user);
                        services.Authentication.SetAccessToken(apiClient.AccessToken);
                    }

                    if (services.Authentication.AuthenticationResult == null)
                    {
                        services.Authentication.SetAccessToken(apiClient.AccessToken);
                    }

                    await services.StartUp.Startup();

                    services.UiInteractions.NavigationService.NavigateToHome();
                    services.UiInteractions.NavigationService.ClearBackStack();
                    break;
                case ConnectionState.ConnectSignIn:
                    services.UiInteractions.NavigationService.NavigateToConnectFirstRun();
                    break;
            }
        }

        public static bool UsePinLogin(DeviceFamily deviceFamily)
        {
            return deviceFamily == DeviceFamily.Xbox || deviceFamily == DeviceFamily.IoT;
        }
    }
}
