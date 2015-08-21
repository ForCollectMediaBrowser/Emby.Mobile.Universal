using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Helpers
{
    public static class ConnectHelper
    {
        public const string DefaultServerConnection = "DefaultServerConnection";

        public async static Task HandleConnectState(ConnectionResult result, IServices services)
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

                    break;
                case ConnectionState.SignedIn:

                    break;
                case ConnectionState.ConnectSignIn:
                    services.NavigationService.NavigateToConnectFirstRun();
                    break;
            }
        }
    }
}
