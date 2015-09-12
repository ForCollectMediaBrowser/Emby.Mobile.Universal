using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Dto;
using System.Threading.Tasks;
using System;
using System.Windows.Input;
using Emby.Mobile.Core.Helpers;
using MediaBrowser.Model.Net;
using MediaBrowser.Model.Entities;
using GalaSoft.MvvmLight.Messaging;
using Emby.Mobile.Core.Strings;

namespace Emby.Mobile.ViewModels.Entities
{
    public class UserDtoViewModel : ViewModelBase, ICanSignIn
    {
        private const string UserProfileSelectedMessage = "UserProfileSelected";

        public string UserImageUrl { get; } = string.Empty;
        public string ErrorMessage { get; set; }
        public bool DisplayErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
        public ICommand SignUpCommand { get; } = null;
        public bool IsEmbyConnect { get; } = false;
        public UserDto UserDto { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } = string.Empty;
        public bool ShowPasswordInput { get; set; } = false;
        public bool CanSignIn => !ProgressIsVisible
                                 && !string.IsNullOrWhiteSpace(Password);

        public DateTime? LastActivity => UserDto?.LastActivityDate;
        public RelayCommand UserTappedCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    Services.Messenger.SendNotification(UserProfileSelectedMessage, this);
                    if (UserDto?.HasPassword == true)
                    {
                        ShowPasswordInput = true;
                    }
                    else
                    {
                        if (!await Authenticate(Username, ""))
                        {
                            ErrorMessage = Resources.ErrorSigningIn;
                        }
                    }
                });
            }
        }

        public ICommand SignInCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (!CanSignIn)
                    {
                        return;
                    }
                    ErrorMessage = string.Empty;
                    UpdateProperties();

                    if (await Authenticate(Username, Password))
                    {
                        ShowPasswordInput = false;
                    }
                    else
                    {
                        ErrorMessage = Resources.ErrorUnableToSignIn;
                    }
                    Password = string.Empty;
                    UpdateProperties();
                });
            }
        }

        public UserDtoViewModel(IServices services, UserDto userDto) : base(services)
        {
            UserDto = userDto;
            Username = UserDto?.Name;

            if (UserDto?.HasPrimaryImage == true)
            {
                UserImageUrl = ApiClient?.GetUserImageUrl(UserDto.Id, ImageOptionsHelper.UserProfile);
            }
        }

        protected override void WireMessages()
        {
            MessengerInstance.Register<NotificationMessage>(this, m =>
            {
                if (m.Notification == UserProfileSelectedMessage && m.Sender != this)
                {
                    ShowPasswordInput = false;
                }
            });

            base.WireMessages();
        }

        private async Task<bool> Authenticate(string username, string password)
        {
            var success = false;
            try
            {
                SetProgressBar(Resources.SysTraySigningIn);

                if (await AuthenticationService.SignIn(username, password))
                {
                    success = true;
                    Services.UiInteractions.NavigationService.NavigateToHome();
                    Services.UiInteractions.NavigationService.ClearBackStack();
                }
            }
            catch (HttpException ex)
            {
                success = false;
            }
            finally
            {
                SetProgressBar();
            }
            return success;
        }

        protected override void UpdateProperties()
        {
            RaisePropertyChanged(() => CanSignIn);
        }
    }
}
