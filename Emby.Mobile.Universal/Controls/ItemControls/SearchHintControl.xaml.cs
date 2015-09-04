using Emby.Mobile.ViewModels.Entities;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Controls.ItemControls
{
    public sealed partial class SearchHintControl : UserControl
    {
        private SearchHintViewModel SearchHint => DataContext as SearchHintViewModel;

        public SearchHintControl()
        {
            InitializeComponent();
            DataContextChanged += (sender, args) =>
            {
                if (DataContext != null)
                    Bindings.Update();
            };
        }        
    }
}
