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
        IMessageBoxService MessageBox { get; }
    }
}
