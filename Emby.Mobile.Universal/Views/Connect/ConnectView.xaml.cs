using Emby.Mobile.ViewModels;
using Emby.Mobile.ViewModels.Connect;

namespace Emby.Mobile.Universal.Views.Connect
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConnectView 
    {
        public ConnectView()
        {
            this.InitializeComponent();
        }

        private ConnectViewModel ConnectViewModel => DataContext as ConnectViewModel;
    }
}
