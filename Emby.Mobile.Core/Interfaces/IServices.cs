using Cimbalino.Toolkit.Services;
using MediaBrowser.Model.Logging;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IServices
    {
        IUIInteractions UiInteractions { get; }
        IServerInteractions ServerInteractions { get; }
        ILogger Log { get; }
        IApplicationSettingsService ApplicationSettings { get; }
        IStorageService Storage { get; }
        IDispatcherService Dispatcher { get; }
        IMessengerService Messenger { get; }
        IDeviceInfoService Device { get; }
        IAnalyticsService Analytics { get; }
        IPlaybackService Playback { get; }
        IStartUpService StartUp { get; }
        ISettingsService Settings { get; }
    }
}
