using System;
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

            TextChanged -= OnTextChanged;
            TextChanged += OnTextChanged;

            MovePlaceholder();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            MovePlaceholder();
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            _storyboard?.Begin();
        }

        private void MovePlaceholder()
        {
            Header = string.IsNullOrEmpty(Text) ? " " : PlaceholderText;
        }
    }
}
