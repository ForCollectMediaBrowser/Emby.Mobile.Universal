using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Emby.Mobile.Universal.Controls
{
    public sealed class EmbyTextBox : TextBox
    {
        private Storyboard _storyboard;

        public EmbyTextBox()
        {
            DefaultStyleKey = typeof(EmbyTextBox);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _storyboard = GetTemplateChild("LostFocusStory") as Storyboard;

            MovePlaceholder();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            _storyboard?.Begin();

            base.OnLostFocus(e);
        }

        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            MovePlaceholder();

            base.OnKeyUp(e);
        }

        private void MovePlaceholder()
        {
            Header = string.IsNullOrEmpty(Text) ? " " : PlaceholderText;
        }
    }
}
