using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Emby.Mobile.Universal.Controls
{
    [TemplateVisualState(GroupName = WatermarkPositions, Name = InText)]
    [TemplateVisualState(GroupName = WatermarkPositions, Name = InHeader)]
    public sealed class EmbyTextBox : TextBox
    {
        private const string WatermarkPositions = "WatermarkPositions";
        private const string InText = "InText";
        private const string InHeader = "InHeader";

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
            var state = string.IsNullOrEmpty(Text) ? InText : InHeader;
            VisualStateManager.GoToState(this, state, true);
        }
    }
}
