using System;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullServerInfoService : IServerInfoService
    {
        public bool HasServer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ServerInfo ServerInfo
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<ServerInfo> ServerInfoChanged;

        public void SetServerInfo(ServerInfo serverInfo)
        {
            throw new NotImplementedException();
        }
    }
}