using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Universal.Views.Connect
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EmbyConnectView 
    {
        public EmbyConnectView()
        {
            this.InitializeComponent();
        }

        private ICanLogin CanLogin => DataContext as ICanLogin;
    }
}
