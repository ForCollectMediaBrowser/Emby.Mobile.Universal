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
        public const string LoggedInUserSetting = "LoggedInUserSettingKey";
        public const string AuthenticationResultSetting = "AuthenticationResultSettingKey";
        public const string ConnectUser = "ConnectUserKey";

        private readonly IConnectionManager _connectionManager;
        private readonly IMessengerService _messengerService;
        private readonly IApplicationSettingsServiceHandler _settingsService;
        private readonly IServerInfoService _serverInfoService;
        private readonly ILogger _logger;
        private readonly IDispatcherService _dispatcher;

        public event EventHandler UserChanged;
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
            _serverInfoService = serverInfoService;
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
            _dispatcher.RunAsync(() => SignedInUser = null);
        }

        private void ConnectionManagerOnLocalUserSignIn(object sender, GenericEventArgs<UserDto> e)
        {
            _dispatcher.RunAsync(() =>
            {
                SetUser(e.Argument);
                if (AuthenticationResult != null)
                {
                    _connectionManager.CurrentApiClient?.SetAuthenticationInfo(AuthenticationResult.AccessToken, SignedInUserId);
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
            var user = _settingsService.SafeGet<UserDto>(LoggedInUserSetting);
            var oldUser = _settingsService.SafeGet<AuthenticationResult>(AuthenticationResultSetting);
            var connectUser = _settingsService.SafeGet<ConnectUser>(ConnectUser);

            if (user != null)
            {
                SetUser(user);
            }

            if (oldUser != null)
            {
                AuthenticationResult = oldUser;
                SetAuthenticationInfo();
            }

            if (connectUser != null)
            {
                SetConnectUser(connectUser);
            }
        }

        public async Task<bool> SignIn(string selectedUserName, string pinCode)
        {
            var success = false;
            try
            {
                _logger.Info("Authenticating user [{0}]", selectedUserName);

                var result = await _connectionManager.CurrentApiClient.AuthenticateUserAsync(selectedUserName, pinCode);

                _logger.Info("Logged in as [{0}]", selectedUserName);

                AuthenticationResult = result;
                _settingsService.SafeSet(AuthenticationResultSetting, AuthenticationResult);

                SetUser(result.User);
                _logger.Info("User [{0}] has been saved", selectedUserName);

                success = true;
            }
            catch (HttpException ex)
            {
                _logger.ErrorException("Login()", ex);
            }

            return success;
        }

        public void SetAuthenticationInfo()
        {
            if (!string.IsNullOrEmpty(AuthenticationResult?.User?.Id))
            {
                _connectionManager.CurrentApiClient?.ClearAuthenticationInfo();
                _connectionManager.CurrentApiClient?.SetAuthenticationInfo(AuthenticationResult.AccessToken, AuthenticationResult.User.Id);
            }
        }

        public void ClearLoggedInUser()
        {
            SignedInUser = null;
            _settingsService.Remove(LoggedInUserSetting);
        }

        public async Task<bool> SignOut(bool removeServerInfo)
        {
            var success = false;

            try
            {
                await _connectionManager.Logout();
                ClearUserData();
                _messengerService.SendAppResetNotification();

                if (removeServerInfo)
                {
                    _serverInfoService.Clear();
                }

                success = true;
            }
            catch (HttpException ex)
            {
                _logger.ErrorException("SignOut()", ex);
            }

            return success;
        }

        private void ClearUserData()
        {
            SignedInUser = null;
            LoggedInConnectUser = null;
            AuthenticationResult = null;
            _messengerService.SendNotification("ClearNowPlayingMsg");

            _settingsService.Remove(LoggedInUserSetting);
            _settingsService.Remove(AuthenticationResultSetting);
            _settingsService.Remove(ConnectUser);
        }

        public UserDto SignedInUser { get; private set; }

        public bool IsSignedIn => SignedInUser != null;

        public string SignedInUserId => SignedInUser?.Id;

        public bool SignedInUsingConnect => LoggedInConnectUser != null;

        public ConnectUser LoggedInConnectUser { get; private set; }

        public async Task<bool> SignInWithConnect(string username, string password)
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
            SignedInUser = user;

            if (user == null)
            {
                _settingsService.Remove(LoggedInUserSetting);
            }
            else
            {
                _settingsService.SafeSet(LoggedInUserSetting, SignedInUser);
            }

            SendUserChangedEvent();
        }

        public void SetConnectUser(ConnectUser connectUser)
        {
            LoggedInConnectUser = connectUser;

            if (connectUser == null)
            {
                _settingsService.Remove(ConnectUser);
            }
            else
            {
                _settingsService.SafeSet(ConnectUser, LoggedInConnectUser);
            }

            SendUserChangedEvent();
        }

        public void SetAccessToken(string accessToken)
        {
            // This is needed for audioplayer to authenticate itself
            var authInfo = new AuthenticationResult
            {
                AccessToken = accessToken,
                User = SignedInUser
            };

            _settingsService.SafeSet(AuthenticationResultSetting, authInfo);

            AuthenticationResult = authInfo;
        }

        private void SendUserChangedEvent()
        {
            var eventHandler = UserChanged;
            eventHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}
