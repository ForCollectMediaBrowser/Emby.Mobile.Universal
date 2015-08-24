using System.Threading.Tasks;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Connect;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Users;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IAuthenticationService
    {
        AuthenticationResult AuthenticationResult { get; }
        UserDto LoggedInUser { get; }
        bool IsLoggedIn { get; }
        string LoggedInUserId { get; }
        bool SignedInUsingConnect { get; }
        ConnectUser LoggedInConnectUser { get; }
        void Start();
        void CheckIfUserSignedIn();
        Task Login(string selectedUserName, string pinCode);
        void SetAuthenticationInfo();
        void ClearLoggedInUser();
        Task SignOut();
        void Logout();
        Task<bool> LoginWithConnect(string username, string password);
        Task<ConnectSignupResponse> SignUpForConnect(string email, string username, string password);
        void SetUser(UserDto user);
        void SetConnectUser(ConnectUser connectUser);
        void SetAccessToken(string accessToken);
    }
}
