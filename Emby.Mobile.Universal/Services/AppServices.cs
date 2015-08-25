using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Core.Helpers;
using Emby.Mobile.Universal.Core.Implementations;
using Emby.Mobile.Universal.Core.Implementations.Connection;
using Emby.Mobile.Universal.Core.Logging;
using Emby.Mobile.Universal.Core.NullServices;
using Emby.Mobile.Universal.Core.NullServices.Cimbalino;
using Emby.Mobile.Universal.Core.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MediaBrowser.ApiInteraction;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Logging;
using Microsoft.Practices.ServiceLocation;
using ScottIsAFool.Windows.MvvmLight.Extensions;
using INavigationService = Emby.Mobile.Core.Interfaces.INavigationService;
using Emby.Mobile.Universal.Strings;

namespace Emby.Mobile.Universal.Services
{
    public static class AppServices
    {
        static AppServices()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                AddDesignTimeServices();
            }
            else
            {
                AddRuntimeServices();
            }
        }

        public static void Create()
        {
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

            SimpleIoc.Default.RegisterIf<IServices, ServicesContainer>();
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

            SimpleIoc.Default.RegisterIf<ILocalizedResources, LocalizedResources>();
            SimpleIoc.Default.RegisterIf<INavigationService, NavigationService>();
            SimpleIoc.Default.RegisterIf<IMessageBoxService, MessageBoxService>();
            SimpleIoc.Default.RegisterIf<IServerInfoService, ServerInfoService>();
            SimpleIoc.Default.RegisterIf<IApplicationSettingsService, ApplicationSettingsService>();
            SimpleIoc.Default.RegisterIf<IStorageService, StorageService>();
            SimpleIoc.Default.RegisterIf<IDispatcherService, DispatcherService>();
            SimpleIoc.Default.RegisterIf<IMessengerService, MessengerService>();
            SimpleIoc.Default.RegisterIf<IAuthenticationService, AuthenticationService>();
            SimpleIoc.Default.RegisterIf<ILauncherService, LauncherService>();

            await AddConnectionServices(device, mbLogger, network);

        }

        private static async Task AddConnectionServices(IDevice device, ILogger mbLogger, INetworkConnection network)
        {
            var connectionManager = await ConnectionManagerFactory.CreateConnectionManager(device, mbLogger, network);
            SimpleIoc.Default.RegisterIf<IConnectionManager>(() => connectionManager);
        }

        private static void AddDesignTimeServices()
        {
            SimpleIoc.Default.RegisterIf<ILogger, MediaBrowser.Model.Logging.NullLogger>();
            SimpleIoc.Default.RegisterIf<ILog, Core.Logging.NullLogger>();
            SimpleIoc.Default.RegisterIf<ILocalizedResources, LocalizedResources>();
            SimpleIoc.Default.RegisterIf<INavigationService, NullNavigationService>();
            SimpleIoc.Default.RegisterIf<IConnectionManager, NullConnectionManager>();
            SimpleIoc.Default.RegisterIf<IMessageBoxService, NullMessageBoxService>();
            SimpleIoc.Default.RegisterIf<IServerInfoService, NullServerInfoService>();
            SimpleIoc.Default.RegisterIf<IApplicationSettingsService, NullApplicationSettingsService>();
            SimpleIoc.Default.RegisterIf<IStorageService, NullStorageService>();
            SimpleIoc.Default.RegisterIf<IDispatcherService, NullDispatcherService>();
            SimpleIoc.Default.RegisterIf<IMessengerService, NullMessengerService>();
            SimpleIoc.Default.RegisterIf<IAuthenticationService, NullAuthenticationService>();
            SimpleIoc.Default.RegisterIf<ILauncherService, NullLauncherService>();
        }

        public static INavigationService NavigationService => ServiceLocator.Current.GetInstance<INavigationService>();
    }
}
