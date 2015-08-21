using Cimbalino.Toolkit.Services;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Logging;
using INavigationService = Emby.Mobile.Core.Interfaces.INavigationService;

namespace Emby.Mobile.Universal.Services
{
    public class ServicesContainer : IServices
    {
        public ServicesContainer(ILogger log, INavigationService navigationService, IConnectionManager connectionManager, IMessageBoxService messageBox)
        {
            Log = log;
            NavigationService = navigationService;
            ConnectionManager = connectionManager;
            MessageBox = messageBox;
        }

        public ILogger Log { get; }
        public INavigationService NavigationService { get; }
        public IConnectionManager ConnectionManager { get; }
        public IMessageBoxService MessageBox { get; }
    }
}
