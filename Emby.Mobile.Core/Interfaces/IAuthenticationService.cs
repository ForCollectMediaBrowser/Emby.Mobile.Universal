﻿using System.Threading.Tasks;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Connect;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Users;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IAuthenticationService
    {
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
        Task<bool> SignOut();
        Task<bool> SignInWithConnect(string username, string password);
        Task<bool> SignInWithPin(string pin);
        Task<ConnectSignupResponse> SignUpForConnect(string email, string username, string password);
        void SetUser(UserDto user);
        void SetConnectUser(ConnectUser connectUser);
        void SetAccessToken(string accessToken);
    }
}
