using System;
using System.Threading.Tasks;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Connect;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Users;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IAuthenticationService
    {
        event EventHandler UserChanged;
        AuthenticationResult AuthenticationResult { get; }
        UserDto SignedInUser { get; }
        bool IsSignedIn { get; }
        string SignedInUserId { get; }
        bool SignedInUsingConnect { get; }
        ConnectUser LoggedInConnectUser { get; }
        void Start();
        void CheckIfUserSignedIn();
        Task<bool> SignIn(string selectedUserName, string pinCode);
        void SetAuthenticationInfo();
        void ClearLoggedInUser();
        Task<bool> SignOut(bool removeServerInfo);
        Task<bool> SignInWithConnect(string username, string password);
        Task<ConnectSignupResponse> SignUpForConnect(string email, string username, string password);
        void SetUser(UserDto user);
        void SetConnectUser(ConnectUser connectUser);
        void SetAccessToken(string accessToken);
    }
}
