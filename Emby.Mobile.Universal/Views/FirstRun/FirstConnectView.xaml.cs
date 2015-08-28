using Emby.Mobile.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Views.FirstRun
{
    public sealed partial class FirstConnectView : Page
    {
        private EmbyConnectViewModel EmbyConnect => DataContext as EmbyConnectViewModel;

        public FirstConnectView()
        {
            InitializeComponent();
        }

       
    }
}
