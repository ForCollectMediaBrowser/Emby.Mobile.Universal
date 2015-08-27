using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.ViewModels
{
    public class EmbyConnectSignUpViewModel : PageViewModelBase
    {
        public EmbyConnectSignUpViewModel(IServices services) : base(services)
        {
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }

        public bool IsValidUsername => !string.IsNullOrWhiteSpace(Username)
                                       && !Username.Contains(" ");

        public bool IsValidPassword => !string.IsNullOrWhiteSpace(Password)
                                       && Password.Length >= 8;

        public bool IsValidEmailAddress => !string.IsNullOrWhiteSpace(EmailAddress)
                                           && EmailAddress.IsValidEmail();

        public string ErrorMessage { get; set; }

        public bool DisplayErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public bool CanSignUp => !ProgressIsVisible
                                 && IsValidUsername
                                 && IsValidPassword
                                 && IsValidEmailAddress;

        public RelayCommand SignUpCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (!CanSignUp)
                    {
                        return;
                    }

                    ErrorMessage = string.Empty;
                    SetProgressBar("SysTraySigningUp");

                    try
                    {
                        var response = await AuthenticationService.SignUpForConnect(EmailAddress, Username, Password);
                        switch (response)
                        {
                            case ConnectSignupResponse.Success:
                                await Services.MessageBox.ShowAsync(Core.Strings.Resources.MessageSignUpSuccessful, Core.Strings.Resources.MessageTitleSuccess, new[] { Core.Strings.Resources.ButtonOk });
                                Services.NavigationService.NavigateToEmbyConnect();
                                Services.NavigationService.ClearBackStack();
                                Reset();
                                break;
                            case ConnectSignupResponse.EmailInUse:
                                ErrorMessage = Core.Strings.Resources.ErrorEmailInUse;
                                break;
                            case ConnectSignupResponse.UsernameInUser:
                                ErrorMessage = Core.Strings.Resources.ErrorUsernameInUse;
                                break;
                            default:
                                ErrorMessage = Core.Strings.Resources.ErrorSigningUp;
                                break;
                        }
                    }
                    catch (HttpException ex)
                    {
                        ErrorMessage = Core.Strings.Resources.ErrorSigningUp;
                        //Utils.HandleHttpException("SignUpCommand", ex, NavigationService, Log);
                    }

                    SetProgressBar();
                });
            }
        }

        protected override void UpdateProperties()
        {
            RaisePropertyChanged(() => CanSignUp);
            base.UpdateProperties();
        }

        private void Reset()
        {
            ErrorMessage = Username = EmailAddress = Password = string.Empty;
        }
    }
}
