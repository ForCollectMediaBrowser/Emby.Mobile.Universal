using System;
using System.Net;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Connect;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Events;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Net;
using MediaBrowser.Model.Users;
using PropertyChanged;

namespace Emby.Mobile.Universal.Core.Services
{
    [ImplementPropertyChanged]
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConnectionManager _connectionManager;
        private readonly IMessengerService _messengerService;
        private readonly IApplicationSettingsServiceHandler _settingsService;
        private readonly ILogger _logger;
        private readonly IDispatcherService _dispatcher;

        public AuthenticationResult AuthenticationResult { get; private set; }

        public AuthenticationService(
            IConnectionManager connectionManager,
            IApplicationSettingsService settingsService,
            IServerInfoService serverInfoService,
            ILogger logger,
            IDispatcherService dispatcher,
            IMessengerService messengerService)
        {
            _settingsService = settingsService.Roaming;
            _connectionManager = connectionManager;
            _logger = logger;
            _dispatcher = dispatcher;
            _messengerService = messengerService;

            _connectionManager.ConnectUserSignIn += ConnectionManagerOnConnectUserSignIn;
            _connectionManager.ConnectUserSignOut += ConnectionManagerOnConnectUserSignOut;
            _connectionManager.LocalUserSignIn += ConnectionManagerOnLocalUserSignIn;
            _connectionManager.LocalUserSignOut += ConnectionManagerOnLocalUserSignOut;
            _connectionManager.Connected += ConnectionManagerOnConnected;
            serverInfoService.ServerInfoChanged += ServerInfoServiceOnServerInfoChanged;

            if (serverInfoService.HasServer)
            {
                SetUserUpdateHandler(serverInfoService.ServerInfo);
            }
        }

        private void ConnectionManagerOnConnected(object sender, GenericEventArgs<ConnectionResult> e)
        {
            var apiClient = e.Argument.ApiClient;
            if (apiClient != null)
            {
                apiClient.UserUpdated -= ApiClientOnUserUpdated;
                apiClient.UserUpdated += ApiClientOnUserUpdated;
            }
        }

        private void ApiClientOnUserUpdated(object sender, GenericEventArgs<UserDto> e)
        {
            _dispatcher.RunAsync(() =>
            {
                SetUser(e.Argument);

                var apiClient = sender as IApiClient;
                if (apiClient != null)
                {
                    SetAccessToken(apiClient.AccessToken);
                }
            });
        }

        private void ServerInfoServiceOnServerInfoChanged(object sender, ServerInfo serverInfo)
        {
            SetUserUpdateHandler(serverInfo);
        }

        private void SetUserUpdateHandler(ServerInfo serverInfo)
        {
            var apiClient = _connectionManager.GetApiClient(serverInfo?.Id);
            if (apiClient != null)
            {
                apiClient.UserUpdated -= ApiClientOnUserUpdated;
                apiClient.UserUpdated += ApiClientOnUserUpdated;
            }
        }

        private void ConnectionManagerOnLocalUserSignOut(object sender, EventArgs eventArgs)
        {
            _dispatcher.RunAsync(() => LoggedInUser = null);
        }

        private void ConnectionManagerOnLocalUserSignIn(object sender, GenericEventArgs<UserDto> e)
        {
            _dispatcher.RunAsync(() =>
            {
                SetUser(e.Argument);
                if (AuthenticationResult != null && _connectionManager.CurrentApiClient != null)
                {
                    _connectionManager.CurrentApiClient.SetAuthenticationInfo(AuthenticationResult.AccessToken, LoggedInUserId);
                }
            });
        }

        private void ConnectionManagerOnConnectUserSignOut(object sender, EventArgs eventArgs)
        {
            _dispatcher.RunAsync(() => LoggedInConnectUser = null);
        }

        private void ConnectionManagerOnConnectUserSignIn(object sender, GenericEventArgs<ConnectUser> e)
        {
            _dispatcher.RunAsync(() => LoggedInConnectUser = e.Argument);
        }

        public void Start()
        {
            CheckIfUserSignedIn();
        }

        public void CheckIfUserSignedIn()
        {
            var user = _settingsService.SafeGet<UserDto>(Constants.Settings.LoggedInUserSetting);
            var oldUser = _settingsService.SafeGet<AuthenticationResult>(Constants.Settings.AuthenticationResultSetting);

            if (user != null)
            {
                SetUser(user);
            }

            if (oldUser != null)
            {
                AuthenticationResult = oldUser;
            }
        }

        public async Task Login(string selectedUserName, string pinCode)
        {
            try
            {
                _logger.Info("Authenticating user [{0}]", selectedUserName);

                var result = await _connectionManager.CurrentApiClient.AuthenticateUserAsync(selectedUserName, pinCode);

                _logger.Info("Logged in as [{0}]", selectedUserName);

                AuthenticationResult = result;
                _settingsService.SafeSet(Constants.Settings.AuthenticationResultSetting, AuthenticationResult);

                SetUser(result.User);
                _logger.Info("User [{0}] has been saved", selectedUserName);
            }
            catch (HttpException ex)
            {
                _logger.ErrorException("Login()", ex);
            }
        }

        public void SetAuthenticationInfo()
        {
            if (!string.IsNullOrEmpty(AuthenticationResult?.User?.Id))
            {
                _connectionManager.CurrentApiClient.ClearAuthenticationInfo();
                _connectionManager.CurrentApiClient.SetAuthenticationInfo(AuthenticationResult.AccessToken, AuthenticationResult.User.Id);
            }
        }

        public void ClearLoggedInUser()
        {
            LoggedInUser = null;
            _settingsService.Remove(Constants.Settings.LoggedInUserSetting);
        }

        public async Task SignOut()
        {
            try
            {
                await _connectionManager.Logout();
                Logout();
                _messengerService.SendAppResetNotification();
            }
            catch (HttpException ex)
            {
                _logger.ErrorException("SignOut()", ex);
            }
        }

        public void Logout()
        {
            LoggedInUser = null;
            LoggedInConnectUser = null;
            AuthenticationResult = null;
            _messengerService.SendNotification("ClearNowPlayingMsg");

            _settingsService.Remove(Constants.Settings.LoggedInUserSetting);
            _settingsService.Remove(Constants.Settings.AuthenticationResultSetting);
            _settingsService.Remove(Constants.Settings.DefaultServerSetting);
        }

        public UserDto LoggedInUser { get; private set; }

        public bool IsLoggedIn => LoggedInUser != null;

        public string LoggedInUserId => LoggedInUser?.Id;

        public bool SignedInUsingConnect => LoggedInConnectUser != null && LoggedInUser != null && LoggedInConnectUser.Id == LoggedInUser.ConnectUserId;

        public ConnectUser LoggedInConnectUser { get; private set; }

        public async Task<bool> LoginWithConnect(string username, string password)
        {
            try
            {
                await _connectionManager.LoginToConnect(username, password);
                return true;
            }
            catch (HttpException ex)
            {
                _logger.ErrorException("Error logging into MB Connect\n", ex);
                return false;
            }
            catch (WebException wex)
            {
                _logger.ErrorException("Error logging into MB Connect\n", wex);
                return false;
            }
            catch (Exception eex)
            {
                _logger.ErrorException("Error logging into MB Connect\n", eex);
                return false;
            }
        }

        public async Task<ConnectSignupResponse> SignUpForConnect(string email, string username, string password)
        {
            var response = await _connectionManager.SignupForConnect(email, username, password);

            return response;
        }

        public void SetUser(UserDto user)
        {
            LoggedInUser = user;

            _settingsService.SafeSet(Constants.Settings.LoggedInUserSetting, LoggedInUser);
        }

        public void SetAccessToken(string accessToken)
        {
            // This is needed for audioplayer to authenticate itself
            var authInfo = new AuthenticationResult
            {
                AccessToken = accessToken,
                User = LoggedInUser
            };

            _settingsService.SafeSet(Constants.Settings.AuthenticationResultSetting, authInfo);

            AuthenticationResult = authInfo;
        }
    }
}
