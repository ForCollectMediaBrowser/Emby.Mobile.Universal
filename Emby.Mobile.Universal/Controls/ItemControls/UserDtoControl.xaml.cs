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
            InitializeComponent();
            DataContextChanged += (sender, args) => Bindings.Update();
        }

    }
}
