using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Strings;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.ViewModels.Connect
{
    public class ConnectSignUpViewModel : PageViewModelBase
    {
        public ConnectSignUpViewModel(IServices services) : base(services)
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
                                await Services.UiInteractions.MessageBox.ShowAsync(Resources.MessageSignUpSuccessful, Resources.MessageTitleSuccess, new[] { Resources.ButtonOk });
                                Services.UiInteractions.NavigationService.NavigateToEmbyConnect();
                                Services.UiInteractions.NavigationService.ClearBackStack();
                                Reset();
                                break;
                            case ConnectSignupResponse.EmailInUse:
                                ErrorMessage = Resources.ErrorEmailInUse;
                                break;
                            case ConnectSignupResponse.UsernameInUser:
                                ErrorMessage = Resources.ErrorUsernameInUse;
                                break;
                            default:
                                ErrorMessage = Resources.ErrorSigningUp;
                                break;
                        }
                    }
                    catch (HttpException ex)
                    {
                        ErrorMessage = Resources.ErrorSigningUp;
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
