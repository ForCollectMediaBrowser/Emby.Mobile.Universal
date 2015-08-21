using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Logging;

namespace Emby.Mobile.Universal.Services
{
    public class ServicesContainer : IServices
    {
        public ServicesContainer(ILogger log, INavigationService navigation, IConnectionManager connection)
        {
            Log = log;
            Navigation = navigation;
            Connection = connection;
        }

        public ILogger Log { get; }
        public INavigationService Navigation { get; }
        public IConnectionManager Connection { get; }
    }
}
