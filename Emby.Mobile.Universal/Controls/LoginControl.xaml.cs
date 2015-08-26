using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Universal.Controls
{
    public sealed partial class LoginControl
    {
        public LoginControl()
        {
            InitializeComponent();
        }

        private ICanSignIn CanSignIn => DataContext as ICanSignIn;
    }
}
