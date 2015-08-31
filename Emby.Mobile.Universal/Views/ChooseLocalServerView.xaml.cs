using Emby.Mobile.ViewModels;

namespace Emby.Mobile.Universal.Views
{
    public sealed partial class ChooseLocalServerView 
    {
        public ChooseLocalServerView()
        {
            InitializeComponent();
        }

        private ChooseServerViewModel ChooseServer => DataContext as ChooseServerViewModel;
    }
}
