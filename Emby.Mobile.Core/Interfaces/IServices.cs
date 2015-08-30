using Cimbalino.Toolkit.Services;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Logging;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IServices
    {
        ILogger Log { get; }
        INavigationService NavigationService { get; }
        IConnectionManager ConnectionManager { get; }
        IAuthenticationService Authentication { get; }
        IMessageBoxService MessageBox { get; }
        IServerInfoService ServerInfo { get; }
        IApplicationSettingsService ApplicationSettings { get; }
        IStorageService Storage { get; }
        IDispatcherService Dispatcher { get; }
        IMessengerService Messenger { get; }
        ILauncherService Launcher { get; }
        IDeviceInfoService Device { get; }
        IAnalyticsService Analytics { get; }
        IStartUpService StartUp { get; }
    }
}
