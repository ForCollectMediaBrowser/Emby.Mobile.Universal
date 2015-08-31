using System;
using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.ApiInteraction;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Connect;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Users;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullAuthenticationService : IAuthenticationService
    {
        public event EventHandler UserChanged;
        public ICredentialProvider Credential { get; } = null;
        public AuthenticationResult AuthenticationResult { get; } = null;
        public UserDto SignedInUser { get; } = null;
        public bool IsSignedIn { get; } = false;
        public string SignedInUserId { get; } = null;
        public bool SignedInUsingConnect { get; } = false;
        public ConnectUser LoggedInConnectUser { get; } = null;
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void CheckIfUserSignedIn()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignIn(string selectedUserName, string pinCode)
        {
            throw new NotImplementedException();
        }

        public void SetAuthenticationInfo()
        {
            throw new NotImplementedException();
        }

        public void ClearLoggedInUser()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignOut(bool removeServerInfo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignInWithConnect(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignInWithPin(string pin)
        {
            throw new NotImplementedException();
        }

        public Task<ConnectSignupResponse> SignUpForConnect(string email, string username, string password)
        {
            throw new NotImplementedException();
        }

        public void SetConnectUser(ConnectUser connectUser)
        {
            throw new NotImplementedException();
        }

        public void SetAccessToken(string accessToken)
        {
            throw new NotImplementedException();
        }

        public void SetUser(UserDto user)
        {
            throw new NotImplementedException();
        }
    }
}
