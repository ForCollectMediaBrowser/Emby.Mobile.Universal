using System;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IServerInfoService
    {
        bool IsOffline { get; }
        bool HasServer { get; }
        ServerInfo ServerInfo { get; }
        void SetServerInfo(ServerInfo serverInfo);
        event EventHandler<ServerInfo> ServerInfoChanged;
        void Save();
        ServerInfo Load();
        void Clear();
    }
}
