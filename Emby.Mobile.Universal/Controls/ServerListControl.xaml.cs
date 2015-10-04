using Emby.Mobile.ViewModels.Connect;

namespace Emby.Mobile.Universal.Controls
{
    public sealed partial class ServerListControl
    {
        private ChooseServerBaseViewModel ChooseServer => DataContext as ChooseServerBaseViewModel;

        public ServerListControl()
        {
            InitializeComponent();

            DataContextChanged += (sender, args) => Bindings.Update();
        }
    }
}
