using Cimbalino.Toolkit.Services;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Logging;
using INavigationService = Emby.Mobile.Core.Interfaces.INavigationService;
using IStatusBarService = Emby.Mobile.Core.Interfaces.IStatusBarService;

namespace Emby.Mobile.Universal.Services
{
    public class ServicesContainer : IServices
    {
        public ServicesContainer(
            ILogger log,
            INavigationService navigationService,
            IConnectionManager connectionManager,
            IMessageBoxService messageBox,
            IServerInfoService serverInfo,
            IApplicationSettingsService applicationSettings,
            IStorageService storage,
            IAuthenticationService authentication,
            IMessengerService messenger,
            IDispatcherService dispatcher,
            ILauncherService launcher,
            IDeviceInfoService device,
            IAnalyticsService analytics,
            IPlaybackService playback,
            IStartUpService startUp,
            IStatusBarService statusBar)
        {
            Log = log;
            NavigationService = navigationService;
            ConnectionManager = connectionManager;
            MessageBox = messageBox;
            ServerInfo = serverInfo;
            ApplicationSettings = applicationSettings;
            Storage = storage;
            Authentication = authentication;
            Messenger = messenger;
            Dispatcher = dispatcher;
            Launcher = launcher;
            Device = device;
            Analytics = analytics;
            Playback = playback;
            StartUp = startUp;
            StatusBar = statusBar;
        }

        public ILogger Log { get; }
        public INavigationService NavigationService { get; }
        public IConnectionManager ConnectionManager { get; }
        public IAuthenticationService Authentication { get; }
        public IMessageBoxService MessageBox { get; }
        public IServerInfoService ServerInfo { get; }
        public IApplicationSettingsService ApplicationSettings { get; }
        public IStorageService Storage { get; }
        public IDispatcherService Dispatcher { get; }
        public IMessengerService Messenger { get; }    
        public ILauncherService Launcher { get; }
        public IDeviceInfoService Device { get; }
        public IAnalyticsService Analytics { get; }
        public IPlaybackService Playback { get; }
        public IStartUpService StartUp { get; }
        public IStatusBarService StatusBar { get; }
    }
}
