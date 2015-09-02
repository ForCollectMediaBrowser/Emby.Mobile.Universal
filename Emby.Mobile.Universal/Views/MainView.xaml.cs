using Emby.Mobile.Universal.Interfaces;
using Emby.Mobile.ViewModels;

namespace Emby.Mobile.Universal.Views
{
    public sealed partial class MainView : ICanHasHeaderMenu
    {
        public MainView()
        {
            InitializeComponent();
        }

        private MainViewModel Main => DataContext as MainViewModel;
    }
}
