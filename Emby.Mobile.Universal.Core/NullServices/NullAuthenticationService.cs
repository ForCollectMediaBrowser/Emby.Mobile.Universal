using System;
using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Connect;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Users;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullAuthenticationService : IAuthenticationService
    {
        public AuthenticationResult AuthenticationResult { get; } = null;
        public UserDto LoggedInUser { get; } = null;
        public bool IsLoggedIn { get; } = false;
        public string LoggedInUserId { get; } = null;
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

        public Task Login(string selectedUserName, string pinCode)
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

        public Task SignOut()
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoginWithConnect(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<ConnectSignupResponse> SignUpForConnect(string email, string username, string password)
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
