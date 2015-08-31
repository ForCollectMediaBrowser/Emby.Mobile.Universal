using System.Threading.Tasks;
using MediaBrowser.ApiInteraction;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Core.Extensions
{
    public static class CredentialProviderExtensions
    {
        public static async Task RemoveServer(this ICredentialProvider credentialProvider, ServerInfo server)
        {
            var servers = await credentialProvider.GetServerCredentials();

            servers.Servers.Remove(server);

            await credentialProvider.SaveServerCredentials(servers);
        }
    }
}
