using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Universal.Controls
{
    public sealed partial class LoginControl
    {
        public LoginControl()
        {
            InitializeComponent();
            //DataContextChanged += (sender, args) =>
            //{
            //    Bindings.Update();
            //};
        }

        private ICanLogin CanLogin => DataContext as ICanLogin;
    }
}
