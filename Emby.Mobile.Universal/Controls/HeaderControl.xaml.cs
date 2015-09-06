using Emby.Mobile.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Controls
{
    public sealed partial class HeaderControl : UserControl
    {
        public HeaderMenuViewModel Header => DataContext as HeaderMenuViewModel;
        public HeaderControl()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                if (Header == null || !Header.IsVisible)
                {
                    VisualStateManager.GoToState(this, "Hide", false);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Show", false);
                }
            };
            DataContextChanged += (s, e) =>
            {
                if (Header != null)
                {
                    Bindings.Update();
                }
            };
        }
    }
}
