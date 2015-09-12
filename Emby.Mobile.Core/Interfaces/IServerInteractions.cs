using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IServerInteractions
    {
        IConnectionManager ConnectionManager { get; }
        IAuthenticationService Authentication { get; }
        IServerInfoService ServerInfo { get; }
    }
}