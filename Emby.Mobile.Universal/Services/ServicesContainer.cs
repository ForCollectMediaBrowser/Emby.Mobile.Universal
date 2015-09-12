using Cimbalino.Toolkit.Services;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Logging;
using INavigationService = Emby.Mobile.Core.Interfaces.INavigationService;
using IStatusBarService = Emby.Mobile.Core.Interfaces.IStatusBarService;

namespace Emby.Mobile.Universal.Services
{
    // ReSharper disable once InconsistentNaming
    public class UIInteractions
    {
        public UIInteractions(
            INavigationService navigationService,
            IMessageBoxService messageBox,
            ILauncherService launcher,
            IStatusBarService statusBar)
        {
            NavigationService = navigationService;
            MessageBox = messageBox;
            Launcher = launcher;
            StatusBar = statusBar;
        }

        public INavigationService NavigationService { get; }
        public ILauncherService Launcher { get; }
        public IMessageBoxService MessageBox { get; }
        public IStatusBarService StatusBar { get; }
    }

    public class ServicesContainer : IServices
    {
        public ServicesContainer(
            ILogger log,
            IConnectionManager connectionManager,
            IServerInfoService serverInfo,
            IApplicationSettingsService applicationSettings,
            IStorageService storage,
            IAuthenticationService authentication,
            IMessengerService messenger,
            IDispatcherService dispatcher,
            IDeviceInfoService device,
            IAnalyticsService analytics,
            IPlaybackService playback,
            IStartUpService startUp,
            ISettingsService settings,
            IUIInteractions uiInteractions)
        {
            Log = log;
            ConnectionManager = connectionManager;
            ServerInfo = serverInfo;
            ApplicationSettings = applicationSettings;
            Storage = storage;
            Authentication = authentication;
            Messenger = messenger;
            Dispatcher = dispatcher;
            Device = device;
            Analytics = analytics;
            Playback = playback;
            StartUp = startUp;
            Settings = settings;
            UiInteractions = uiInteractions;
        }

        public IUIInteractions UiInteractions { get; }
        public ILogger Log { get; }
        public IConnectionManager ConnectionManager { get; }
        public IAuthenticationService Authentication { get; }
        public IServerInfoService ServerInfo { get; }
        public IApplicationSettingsService ApplicationSettings { get; }
        public IStorageService Storage { get; }
        public IDispatcherService Dispatcher { get; }
        public IMessengerService Messenger { get; }    
        public IDeviceInfoService Device { get; }
        public IAnalyticsService Analytics { get; }
        public IPlaybackService Playback { get; }
        public IStartUpService StartUp { get; }
        public ISettingsService Settings { get; set; }
    }
}
