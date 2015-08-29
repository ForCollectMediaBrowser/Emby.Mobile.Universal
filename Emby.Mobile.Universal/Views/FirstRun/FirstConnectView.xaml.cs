using Emby.Mobile.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Views.FirstRun
{
    public sealed partial class FirstConnectView
    {
        private ConnectViewModel EmbyConnect => DataContext as ConnectViewModel;

        public FirstConnectView()
        {
            InitializeComponent();
        }
    }
}
