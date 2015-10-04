using System.Collections.Generic;
using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.ViewModels.Connect
{
    public class ConnectChooseServerViewModel : ChooseServerBaseViewModel
    {
        public ConnectChooseServerViewModel(IServices services) : base(services)
        {
        }
        
        protected override async Task<List<ServerInfo>> GetServers()
        {
            var connect = await Services.ServerInteractions.ConnectionManager.Connect();
            var servers = connect.Servers;

            return servers;
        }
    }
}
