using Emby.Mobile.ViewModels.Entities;

namespace Emby.Mobile.Universal.Controls.ItemControls
{
    public sealed partial class ServerInfoControl
    {
        public ServerInfoControl()
        {
            this.InitializeComponent(); 
        }

        private ServerInfoViewModel ServerInfo => this.DataContext as ServerInfoViewModel;
    }
}
