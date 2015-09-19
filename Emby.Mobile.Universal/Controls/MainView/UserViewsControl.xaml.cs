using Emby.Mobile.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Controls.MainView
{
    public sealed partial class UserViewsControl : UserControl
    {
        public UserViewsControl()
        {
            InitializeComponent();
            DataContextChanged += async (s, e) =>
            {
                if (Views != null)
                {
                    await Views.Refresh();
                    Bindings.Update();
                }
            };
        }

        private UserViewsViewModel Views => DataContext as UserViewsViewModel;
    }
}
