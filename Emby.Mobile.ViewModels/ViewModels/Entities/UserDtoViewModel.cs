﻿using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Dto;
using System.Threading.Tasks;
using System;
using System.Windows.Input;
using MediaBrowser.Model.Net;
using MediaBrowser.Model.Entities;

namespace Emby.Mobile.ViewModels.Entities
{
    public class UserDtoViewModel : ViewModelBase, ICanSignIn
    {
        public string UserImageUrl { get; } = "ms-appx:///Assets/Logo.png";
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
                    if (UserDto?.HasPassword == true)
                    {
                        ShowPasswordInput = true;
                    }
                    else
                    {
                        if (!await Authenticate(Username, ""))
                        {
                            ErrorMessage = GetLocalizedString("ErrorSigningIn");
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
                        ErrorMessage = GetLocalizedString("ErrorUnableToSignIn");
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
                var options = new ImageOptions
                {
                    Quality = 90,
                    Height = 120,
                    ImageType = ImageType.Primary
                };

                UserImageUrl = ApiClient.GetUserImageUrl(UserDto.Id, options);
            }
        }

        private async Task<bool> Authenticate(string username, string password)
        {
            var success = false;
            try
            {
                SetProgressBar(GetLocalizedString("SysTraySigningIn"));

                if (await AuthenticationService.Login(Username, Password))
                {
                    success = true;
                    Services.NavigationService.NavigateToHome();
                    Services.NavigationService.ClearBackStack();
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

        public override void UpdateProperties()
        {
            RaisePropertyChanged(() => DisplayErrorMessage);
        }
    }
}
