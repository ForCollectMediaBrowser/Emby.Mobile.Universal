﻿using System.Windows.Input;
using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.ViewModels
{
    public class ManualLocalUserSignInViewModel : PageViewModelBase, ICanLogin
    {
        public ManualLocalUserSignInViewModel(IServices services) : base(services)
        {
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public bool CanSignIn => !ProgressIsVisible
                                 && !string.IsNullOrEmpty(Username);

        public bool IsEmbyConnect { get; } = false;

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

                    try
                    {
                        SetProgressBar(GetLocalizedString("SysTraySigningIn"));

                        await AuthenticationService.Login(Username, Password);

                        Services.NavigationService.NavigateToHome();
                    }
                    catch (HttpException ex)
                    {

                    }
                    finally
                    {
                        SetProgressBar();
                    }
                });
            }
        }

        public override void UpdateProperties()
        {
            RaisePropertyChanged(() => CanSignIn);
        }
    }
}
