using System.Threading.Tasks;
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
                    await services.MessageBox.ShowAsync("Could not find server");
                    services.NavigationService.NavigateToConnectFirstRun();
                    break;
                case ConnectionState.ServerSelection:
                    services.NavigationService.NavigateToServerSelection();
                    break;
                case ConnectionState.ServerSignIn:
                    // TODO: Manual sign in
                    break;
                case ConnectionState.SignedIn:
                    if (services.Authentication.LoggedInUser == null)
                    {
                        var user = await apiClient.GetUserAsync(apiClient.CurrentUserId);
                        services.Authentication.SetUser(user);
                        services.Authentication.SetAccessToken(apiClient.AccessToken);
                    }

                    if (services.Authentication.AuthenticationResult == null)
                    {
                        services.Authentication.SetAccessToken(apiClient.AccessToken);
                    }

                    services.NavigationService.NavigateToHome();
                    services.NavigationService.ClearBackStack();
                    break;
                case ConnectionState.ConnectSignIn:
                    services.NavigationService.NavigateToConnectFirstRun();
                    break;
            }
        }
    }
}
