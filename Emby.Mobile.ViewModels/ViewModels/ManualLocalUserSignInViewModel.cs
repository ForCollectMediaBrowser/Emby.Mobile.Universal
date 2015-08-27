using System.Windows.Input;
using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.ViewModels
{
    public class ManualLocalUserSignInViewModel : PageViewModelBase, ICanSignIn
    {
        public ManualLocalUserSignInViewModel(IServices services) : base(services)
        {
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public bool CanSignIn => !ProgressIsVisible
                                 && !string.IsNullOrEmpty(Username);

        public ICommand SignUpCommand { get; } = null;
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
                        SetProgressBar(Core.Strings.Resources.SysTraySigningIn);

                        if (await AuthenticationService.Login(Username, Password))
                        {
                            Services.NavigationService.NavigateToHome();
                            Services.NavigationService.ClearBackStack();
                        }
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

        protected override void UpdateProperties()
        {
            RaisePropertyChanged(() => CanSignIn);
        }
    }
}
