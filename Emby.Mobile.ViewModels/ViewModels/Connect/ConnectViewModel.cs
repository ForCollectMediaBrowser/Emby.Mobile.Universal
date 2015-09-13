using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Strings;
using Emby.Mobile.Helpers;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.ViewModels.Connect
{
    public class ConnectViewModel : PageViewModelBase, ICanSignIn
    {
        public ConnectViewModel(IServices services)
            : base(services)
        {
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public bool CanSignIn => !ProgressIsVisible
                                 && !string.IsNullOrWhiteSpace(Username)
                                 && !string.IsNullOrWhiteSpace(Password);

        public bool IsEmbyConnect { get; } = true;

        public ICommand SignInCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await SignIn();
                });
            }
        }

        public ICommand SkipCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Services.UiInteractions.NavigationService.NavigateToLocalServerSelection();
                });
            }
        }

        public ICommand SignUpCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Services.UiInteractions.NavigationService.NavigateToEmbyConnectSignUp();
                });
            }
        }

        private async Task SignIn()
        {
            try
            {
                if (!CanSignIn)
                {
                    return;
                }

                SetProgressBar(Resources.SysTraySigningIn);

                var success = await AuthenticationService.SignInWithConnect(Username, Password);

                if (success)
                {
                    var result = await Services.ServerInteractions.ConnectionManager.Connect();
                    if (result.State == ConnectionState.SignedIn && result.Servers.Count == 1)
                    {
                        Services.ServerInteractions.ServerInfo.SetServerInfo(result.Servers[0]);
                        Services.ServerInteractions.ServerInfo.Save();
                    }

                    AuthenticationService.SetConnectUser(result.ConnectUser);

                    await ConnectHelper.HandleConnectState(result, Services, ApiClient);

                    Services.UiInteractions.NavigationService.RemoveBackEntry();
                }
            }
            catch (HttpException hex)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                SetProgressBar();
            }
        }

        protected override void UpdateProperties()
        {
            RaisePropertyChanged(() => CanSignIn);
        }
    }
}
