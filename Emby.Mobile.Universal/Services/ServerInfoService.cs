using System;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Universal.Services
{
    public class ServerInfoService : IServerInfoService
    {
        public bool HasServer => !string.IsNullOrEmpty(ServerInfo?.Id);

        public ServerInfo ServerInfo { get; private set; }

        public void SetServerInfo(ServerInfo serverInfo)
        {
            ServerInfo = serverInfo;

            SendEvent();
        }

        private void SendEvent()
        {
            var eventHandler = ServerInfoChanged;
            eventHandler?.Invoke(this, ServerInfo);
        }

        public event EventHandler<ServerInfo> ServerInfoChanged;
    }
}
