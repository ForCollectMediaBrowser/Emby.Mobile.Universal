using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Universal.Services
{
    public class ServerInteractions : IServerInteractions
    {
        public ServerInteractions(
            IAuthenticationService authentication,
            IConnectionManager connectionManager,
            IServerInfoService serverInfo)
        {
            Authentication = authentication;
            ConnectionManager = connectionManager;
            ServerInfo = serverInfo;
        }

        public IConnectionManager ConnectionManager { get; }
        public IAuthenticationService Authentication { get; }
        public IServerInfoService ServerInfo { get; }
    }
}