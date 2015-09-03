using Emby.Mobile.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Controls
{
    public sealed partial class HeaderControl : UserControl
    {
        public HeaderMenuViewModel Header => DataContext as HeaderMenuViewModel;
        public HeaderControl()
        {
            InitializeComponent();
            DataContextChanged += (s, e) =>
            {
                if (Header != null)
                    Bindings.Update();
            };
        }
    }
}
