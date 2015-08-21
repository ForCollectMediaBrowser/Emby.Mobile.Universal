using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Core.Helpers;
using Emby.Mobile.Universal.Core.Implementations;
using Emby.Mobile.Universal.Core.Implementations.Connection;
using Emby.Mobile.Universal.Core.Logging;
using Emby.Mobile.Universal.Core.NullServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MediaBrowser.ApiInteraction;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Logging;
using Microsoft.Practices.ServiceLocation;
using ScottIsAFool.Windows.MvvmLight.Extensions;

namespace Emby.Mobile.Universal.Services
{
    public static class AppServices
    {
        public static void Create()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                AddDesignTimeServices();
            }
            else
            {
                // Create run time view services and models
                AddRuntimeServices();
            }
        }

        private async static void AddRuntimeServices()
        {
            var logger = new WinLogger("Emby.Universal");
            var mbLogger = new MBLogger(logger);
            var device = new Device();
            var network = new NetworkConnection();
            SimpleIoc.Default.RegisterIf<ILog>(() => logger);
            SimpleIoc.Default.RegisterIf<IDevice>(() => device);
            SimpleIoc.Default.RegisterIf<INetworkConnection>(() => network);
            SimpleIoc.Default.RegisterIf<ILogger>(() => mbLogger);

            SimpleIoc.Default.RegisterIf<INavigationService, NavigationService>();

            await AddConnectionServices(device, mbLogger, network);
        }

        private static async Task AddConnectionServices(IDevice device, ILogger mbLogger, INetworkConnection network)
        {
            var connectionManager = await ConnectionManagerFactory.CreateConnectionManager(device, mbLogger, network);
            SimpleIoc.Default.RegisterIf<IConnectionManager>(() => connectionManager);
        }

        private static void AddDesignTimeServices()
        {
            SimpleIoc.Default.RegisterIf<INavigationService, NullNavigationService>();
            SimpleIoc.Default.RegisterIf<IConnectionManager, NullConnectionManager>();
        }

        public static INavigationService NavigationService => ServiceLocator.Current.GetInstance<INavigationService>();
    }
}
