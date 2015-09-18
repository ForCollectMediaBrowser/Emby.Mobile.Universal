using Emby.Mobile.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Controls.MainView
{
    public sealed partial class HomeControl : UserControl
    {        
        public HomeControl()
        {
            InitializeComponent();
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                DataContextChanged += async (s, e) =>
                {
                    if (Home != null)
                    {
                        await Home.Refresh();
                        Bindings.Update();
                    }
                    
                };
            }
        }        

        private HomeViewModel Home => DataContext as HomeViewModel;
    }
}
