using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

            LostFocus -= OnLostFocus;
            LostFocus += OnLostFocus;
        }

        private void OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            _storyboard?.Begin();
        }
    }
}
