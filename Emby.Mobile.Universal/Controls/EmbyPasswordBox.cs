using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Controls
{
    [TemplatePart(Name = PasswordBox, Type = typeof(PasswordBox))]
    [TemplateVisualState(GroupName = CommonStatesGroup, Name = FocusedState)]
    [TemplateVisualState(GroupName = CommonStatesGroup, Name = LostFocusState)]
    public sealed class EmbyPasswordBox : Control
    {
        private const string PasswordBox = "PasswordBox";
        private const string CommonStatesGroup = "CommonStates";
        private const string FocusedState = "Focused";
        private const string LostFocusState = "LostFocus";

        private PasswordBox _passwordBox;

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
            "Password", typeof (string), typeof (EmbyPasswordBox), new PropertyMetadata(default(string)));

        public string Password
        {
            get { return (string) GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
            "PlaceholderText", typeof (string), typeof (EmbyPasswordBox), new PropertyMetadata(default(string)));

        public string PlaceholderText
        {
            get { return (string) GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public EmbyPasswordBox()
        {
            DefaultStyleKey = typeof(EmbyPasswordBox);
        }
        
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _passwordBox = GetTemplateChild(PasswordBox) as PasswordBox;

            if (_passwordBox != null)
            {
                _passwordBox.PasswordChanged -= PasswordBoxOnPasswordChanged;
                _passwordBox.PasswordChanged += PasswordBoxOnPasswordChanged;
            }

            MovePlaceholder();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, LostFocusState, true);
            if (_passwordBox != null)
            {
                VisualStateManager.GoToState(_passwordBox, "Normal", true);
            }

            base.OnLostFocus(e);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, FocusedState, true);

            _passwordBox?.Focus(FocusState.Programmatic);

            base.OnGotFocus(e);
        }

        private void PasswordBoxOnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = _passwordBox?.Password;

            MovePlaceholder();
        }

        private void MovePlaceholder()
        {
            _passwordBox.Header = string.IsNullOrEmpty(_passwordBox?.Password) ? " " : PlaceholderText;
        }
    }
}
