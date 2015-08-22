using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.ViewModels.Entities
{
    public class ServerInfoViewModel : ViewModelBase
    {
        public ServerInfoViewModel(IServices services, ServerInfo serverInfo) : base(services)
        {
            ServerInfo = serverInfo;
        }

        public ServerInfo ServerInfo { get; set; }

        public string ServerName => ServerInfo?.Name;

        public string LocalAddress => ServerInfo?.LocalAddress;

        public string ExternalAddress => ServerInfo?.RemoteAddress;

        public bool DisplayLocalAddress => !string.IsNullOrEmpty(LocalAddress);

        public bool DisplayExternalAddress => !string.IsNullOrEmpty(ExternalAddress);
    }
}
