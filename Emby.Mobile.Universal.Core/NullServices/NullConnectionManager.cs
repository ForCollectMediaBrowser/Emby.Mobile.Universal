using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Connect;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Events;
using MediaBrowser.Model.Session;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullConnectionManager : IConnectionManager
    {
        public IApiClient GetApiClient(IHasServerId item)
        {
            throw new NotImplementedException();
        }

        public IApiClient GetApiClient(string serverId)
        {
            throw new NotImplementedException();
        }

        public Task<ConnectionResult> Connect(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<ConnectionResult> Connect(IApiClient apiClient, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<ConnectionResult> Connect(ServerInfo server, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<ConnectionResult> Connect(ServerInfo server, ConnectionOptions options, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<ConnectionResult> Connect(string address, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public Task LoginToConnect(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<PinCreationResult> CreatePin()
        {
            throw new NotImplementedException();
        }

        public Task<PinStatusResult> GetPinStatus(PinCreationResult pin)
        {
            throw new NotImplementedException();
        }

        public Task ExchangePin(PinCreationResult pin)
        {
            throw new NotImplementedException();
        }

        public Task<ServerInfo> GetServerInfo(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ServerInfo>> GetAvailableServers(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task AuthenticateOffline(UserDto user, string password, bool rememberCredentials)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDto>> GetOfflineUsers()
        {
            throw new NotImplementedException();
        }

        public Task<ConnectSignupResponse> SignupForConnect(string email, string username, string password, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public IDevice Device { get; }
        public ConnectUser ConnectUser { get; }
        public bool SaveLocalCredentials { get; set; }
        public ClientCapabilities ClientCapabilities { get; }
        public IApiClient CurrentApiClient { get; }
        public event EventHandler<GenericEventArgs<ConnectionResult>> Connected;
        public event EventHandler<GenericEventArgs<UserDto>> LocalUserSignIn;
        public event EventHandler<GenericEventArgs<ConnectUser>> ConnectUserSignIn;
        public event EventHandler<GenericEventArgs<IApiClient>> LocalUserSignOut;
        public event EventHandler<EventArgs> ConnectUserSignOut;
        public event EventHandler<EventArgs> RemoteLoggedOut;
    }
}
