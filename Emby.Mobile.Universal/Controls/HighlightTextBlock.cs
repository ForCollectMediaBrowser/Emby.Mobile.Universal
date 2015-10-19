using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

// Original control implementation by @igorkulman http://blog.kulman.sk/highlighting-letters-in-textblock-in-windows-phone-8-1-and-windows-8-1/

namespace Emby.Mobile.Universal.Controls
{
    [TemplatePart(Name = TextBlockControl, Type = typeof(TextBlock))]
    public sealed class HighlightTextBlock : Control
    {
        private const string TextBlockControl = "TextBlock";

        private TextBlock _textBlock;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(HighlightTextBlock), new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var c = d as HighlightTextBlock;
            c?.Update();
        }

        public string HighlightedText
        {
            get { return (string)GetValue(HighlightedTextProperty); }
            set { SetValue(HighlightedTextProperty, value); }
        }

        public static readonly DependencyProperty HighlightedTextProperty = DependencyProperty.Register("HighlightedText", typeof(string), typeof(HighlightTextBlock), new PropertyMetadata(null, PropertyChangedCallback));

        public SolidColorBrush HighlightBrush
        {
            get { return (SolidColorBrush)GetValue(HighlightBrushProperty); }
            set { SetValue(HighlightBrushProperty, value); }
        }

        public static readonly DependencyProperty HighlightBrushProperty = DependencyProperty.Register("HighlightBrush", typeof(SolidColorBrush), typeof(HighlightTextBlock), new PropertyMetadata(default(SolidColorBrush), PropertyChangedCallback));

        public static readonly new DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "Foreground", typeof (SolidColorBrush), typeof (HighlightTextBlock), new PropertyMetadata(default(SolidColorBrush), PropertyChangedCallback));

        public new SolidColorBrush Foreground
        {
            get { return (SolidColorBrush) GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty TextBlockStyleProperty = DependencyProperty.Register(
            "TextBlockStyle", typeof (Style), typeof (HighlightTextBlock), new PropertyMetadata(default(Style)));

        public Style TextBlockStyle
        {
            get { return (Style) GetValue(TextBlockStyleProperty); }
            set { SetValue(TextBlockStyleProperty, value); }
        }

        public HighlightTextBlock()
        {
            DefaultStyleKey = typeof(HighlightTextBlock);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _textBlock = GetTemplateChild(TextBlockControl) as TextBlock;

        }

        private void Update()
        {
            if (_textBlock == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(Text) || string.IsNullOrEmpty(HighlightedText))
            {
                _textBlock.Inlines.Clear();
                return;
            }

            _textBlock.Inlines.Clear();
            var parts = Regex.Split(Text, HighlightedText, RegexOptions.IgnoreCase);
            var len = 0;
            foreach (var part in parts)
            {
                len = len + part.Length + 1;

                _textBlock.Inlines.Add(new Run
                {
                    Text = part,
                    Foreground = Foreground
                });

                if (Text.Length >= len)
                {
                    var highlight = Text.Substring(len - 1, HighlightedText.Length); //to match the case

                    _textBlock.Inlines.Add(new Run
                    {
                        Text = highlight,
                        Foreground = HighlightBrush
                    });
                }
            }
        }
    }
}
