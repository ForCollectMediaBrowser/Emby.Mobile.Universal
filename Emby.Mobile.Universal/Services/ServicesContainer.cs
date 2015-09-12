using Cimbalino.Toolkit.Services;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.Logging;

namespace Emby.Mobile.Universal.Services
{
    public class ServicesContainer : IServices
    {
        public ServicesContainer(
            ILogger log,
            IApplicationSettingsService applicationSettings,
            IStorageService storage,
            IMessengerService messenger,
            IDispatcherService dispatcher,
            IDeviceInfoService device,
            IAnalyticsService analytics,
            IPlaybackService playback,
            IStartUpService startUp,
            ISettingsService settings,
            IUIInteractions uiInteractions,
            IServerInteractions serverInteractions)
        {
            Log = log;
            ApplicationSettings = applicationSettings;
            Storage = storage;
            Messenger = messenger;
            Dispatcher = dispatcher;
            Device = device;
            Analytics = analytics;
            Playback = playback;
            StartUp = startUp;
            Settings = settings;
            UiInteractions = uiInteractions;
            ServerInteractions = serverInteractions;
        }

        public IUIInteractions UiInteractions { get; }
        public IServerInteractions ServerInteractions { get; set; }
        public ILogger Log { get; }
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
