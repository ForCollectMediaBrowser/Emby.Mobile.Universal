using System.Collections.Generic;
using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.ViewModels.Connect
{
    public class ChooseServerViewModel : ChooseServerBaseViewModel
    {
        public ChooseServerViewModel(IServices services) : base(services)
        {
        }

        protected override async Task<List<ServerInfo>> GetServers()
        {
            var servers = await Services.ServerInteractions.ConnectionManager.GetAvailableServers();

            return servers;
        }
    }
}