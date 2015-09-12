using Cimbalino.Toolkit.Services;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Logging;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IServices
    {
        IUIInteractions UiInteractions { get; }
        ILogger Log { get; }
        IConnectionManager ConnectionManager { get; }
        IAuthenticationService Authentication { get; }
        IServerInfoService ServerInfo { get; }
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
