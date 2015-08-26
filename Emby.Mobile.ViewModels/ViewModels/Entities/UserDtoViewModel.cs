using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Dto;
using System.Threading.Tasks;

namespace Emby.Mobile.ViewModels.Entities
{
    public class UserDtoViewModel : ViewModelBase
    {
        public string ErrorMessage { get; set; }
        public bool DisplayErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public UserDto UserDto { get; set; }
        public string UserName => UserDto?.Name;
        public string Password { get; set; }
        public bool ShowPasswordInput { get; set; }

        public bool CanSignIn => !ProgressIsVisible
                                 && !string.IsNullOrWhiteSpace(Password);

        public UserDtoViewModel(IServices services, UserDto userDto) : base(services)
        {
            UserDto = userDto;
        }

        public RelayCommand UserTappedCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (UserDto?.HasPassword == true)
                    {
                        ShowPasswordInput = true;
                    }
                    else
                    {
                        if (await Authenticate(UserName, ""))
                            Services.NavigationService.NavigateToHome();
                    }
                });
            }
        }
        public RelayCommand AuthenticateCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (await Authenticate(UserName, Password))
                    {
                        Services.NavigationService.NavigateToHome();
                        ShowPasswordInput = false;
                    }
                    Password = string.Empty;
                });
            }
        }
        private Task<bool> Authenticate(string username, string password)
        {
            SetProgressBar(GetLocalizedString("SysTrayAuthenticating"));
            var authenticated = AuthenticationService.Login(username, password);
            SetProgressBar();
            return authenticated;
        }
    }
}
