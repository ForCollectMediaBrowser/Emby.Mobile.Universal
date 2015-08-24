using System;
using Cimbalino.Toolkit.Services;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Core;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Universal.Services
{
    public class ServerInfoService : IServerInfoService
    {
        private readonly IApplicationSettingsService _applicationSettings;
        public ServerInfoService(IApplicationSettingsService applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }

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
        public void Save()
        {
            _applicationSettings.Roaming.SafeSet(Constants.Settings.DefaultServerSetting, ServerInfo);
        }

        public ServerInfo Load()
        {
            var server = _applicationSettings.Roaming.SafeGet<ServerInfo>(Constants.Settings.DefaultServerSetting);

            SetServerInfo(server);

            return server;
        }
    }
}
