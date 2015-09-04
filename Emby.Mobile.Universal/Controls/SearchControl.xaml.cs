using Emby.Mobile.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Controls
{
    public sealed partial class SearchControl : UserControl
    {
        public SearchViewModel Search => DataContext as SearchViewModel;

        public SearchControl()
        {
            InitializeComponent();
        }
    }
}
