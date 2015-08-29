using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.ViewModels.Entities;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Controls.ItemControls
{
    public sealed partial class UserDtoControl : UserControl
    {
        private UserDtoViewModel User => DataContext as UserDtoViewModel;
        private ICanSignIn CanSignIn => DataContext as ICanSignIn;

        public UserDtoControl()
        {
            DataContextChanged += (sender, args) => Bindings.Update();
            InitializeComponent();
        }

        public void SetFocusToPasswordBox()
        {
            PasswordBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
        }
    }
}
