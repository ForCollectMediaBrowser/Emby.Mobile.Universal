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
using IStatusBarService = Emby.Mobile.Core.Interfaces.IStatusBarService;
using Emby.Mobile.Core.Strings;
using MediaBrowser.ApiInteraction.Data;

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
            var credentials = new CredentialProvider(mbLogger);
            SimpleIoc.Default.RegisterIf<ILog>(() => logger);
            SimpleIoc.Default.RegisterIf<IDevice>(() => device);
            SimpleIoc.Default.RegisterIf<INetworkConnection>(() => network);
            SimpleIoc.Default.RegisterIf<ILogger>(() => mbLogger);
            SimpleIoc.Default.RegisterIf<ICredentialProvider>(() => credentials);

            SimpleIoc.Default.RegisterIf<ILocalizedResources, LocalizedStrings>();
            SimpleIoc.Default.RegisterIf<INavigationService, NavigationService>();
            SimpleIoc.Default.RegisterIf<IMessageBoxService, MessageBoxService>();
            SimpleIoc.Default.RegisterIf<IServerInfoService, ServerInfoService>();
            SimpleIoc.Default.RegisterIf<IApplicationSettingsService, ApplicationSettingsService>();
            SimpleIoc.Default.RegisterIf<IStorageService, StorageService>();
            SimpleIoc.Default.RegisterIf<IDispatcherService, DispatcherService>();
            SimpleIoc.Default.RegisterIf<IMessengerService, MessengerService>();
            SimpleIoc.Default.RegisterIf<IAuthenticationService, AuthenticationService>();
            SimpleIoc.Default.RegisterIf<ILauncherService, LauncherService>();
            SimpleIoc.Default.RegisterIf<IDeviceInfoService, DeviceInfoService>();
            SimpleIoc.Default.RegisterIf<IAnalyticsService, AnalyticsService>();
            SimpleIoc.Default.RegisterIf<IPlaybackService, PlaybackService>();
            SimpleIoc.Default.RegisterIf<IStartUpService, StartUpService>();
            SimpleIoc.Default.RegisterIf<IStatusBarService, StatusBarService>();

            await AddConnectionServices(device, mbLogger, network, credentials);

        }

        private static async Task AddConnectionServices(IDevice device, ILogger mbLogger, INetworkConnection network, ICredentialProvider credentialProvider)
        {
            var connectionManager = await ConnectionManagerFactory.CreateConnectionManager(device, mbLogger, network, credentialProvider);
            SimpleIoc.Default.RegisterIf<IConnectionManager>(() => connectionManager);
        }

        private static void AddPlaybackManager(IDevice device, ILogger mbLogger, INetworkConnection network, ILocalAssetManager localAssetManager)
        { }

        private static void AddDesignTimeServices()
        {
            SimpleIoc.Default.RegisterIf<ILogger, MediaBrowser.Model.Logging.NullLogger>();
            SimpleIoc.Default.RegisterIf<ILog, Core.Logging.NullLogger>();
            SimpleIoc.Default.RegisterIf<ILocalizedResources, LocalizedStrings>();
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
            SimpleIoc.Default.RegisterIf<IDeviceInfoService, NullDeviceInfoService>();
            SimpleIoc.Default.RegisterIf<IAnalyticsService, NullAnalyticsService>();
            SimpleIoc.Default.RegisterIf<IPlaybackService, NullPlaybackService>();
            SimpleIoc.Default.RegisterIf<IStartUpService, NullStartUpService>();
            SimpleIoc.Default.RegisterIf<IStatusBarService, NullStatusBarService>();
            SimpleIoc.Default.RegisterIf<ICredentialProvider, NullCredentialProvider>();
        }

        public static INavigationService NavigationService => ServiceLocator.Current.GetInstance<INavigationService>();
        public static ILogger Log => ServiceLocator.Current.GetInstance<ILogger>();
        public static IAnalyticsService Anayltics => ServiceLocator.Current.GetInstance<IAnalyticsService>();
        public static IDeviceInfoService DeviceInfo => ServiceLocator.Current.GetInstance<IDeviceInfoService>();
        public static ILauncherService LauncherService => ServiceLocator.Current.GetInstance<ILauncherService>();
        public static IPlaybackService PlaybackService => ServiceLocator.Current.GetInstance<IPlaybackService>();
    }
}
