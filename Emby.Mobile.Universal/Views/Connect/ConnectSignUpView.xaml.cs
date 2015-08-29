using Emby.Mobile.ViewModels;

namespace Emby.Mobile.Universal.Views.Connect
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConnectSignUpView
    {
        public ConnectSignUpView()
        {
            InitializeComponent();
        }

        private ConnectSignUpViewModel SignUp => DataContext as ConnectSignUpViewModel;
    }
}
