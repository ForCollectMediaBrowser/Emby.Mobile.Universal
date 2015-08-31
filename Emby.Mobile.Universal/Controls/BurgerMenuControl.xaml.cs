using Emby.Mobile.ViewModels;

namespace Emby.Mobile.Universal.Controls
{
    public sealed partial class BurgerMenuControl
    {
        public BurgerMenuControl()
        {
            InitializeComponent();
            DataContextChanged += (sender, args) => Bindings.Update();
        }

        private BurgerMenuViewModel Burger => DataContext as BurgerMenuViewModel;
    }
}
