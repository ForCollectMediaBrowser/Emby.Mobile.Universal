using System.Threading.Tasks;
using Emby.Mobile.Core.Helpers;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Net;

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
                    services.StartUp.LoadFrame();
                    if (services.ServerInteractions.Authentication.SignedInUser == null)
                    {                        
                        services.UiInteractions.NavigationService.NavigateToChooseProfile();
                    }
                    else
                    {
                        services.ServerInteractions.Authentication.SetAuthenticationInfo();
                        await services.StartUp.Startup();

                        services.UiInteractions.NavigationService.NavigateToHome();
                    }
                    break;
                case ConnectionState.SignedIn:
                    if (services.ServerInteractions.Authentication.SignedInUser == null)
                    {
                        try
                        {
                            var user = await apiClient.GetUserAsync(apiClient.CurrentUserId);
                            services.ServerInteractions.Authentication.SetUser(user);
                            services.ServerInteractions.Authentication.SetAccessToken(apiClient.AccessToken);
                        }
                        catch (HttpException ex)
                        {
                            var i = 0;
                        }
                    }

                    if (services.ServerInteractions.Authentication.AuthenticationResult == null)
                    {
                        services.ServerInteractions.Authentication.SetAccessToken(apiClient.AccessToken);
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
