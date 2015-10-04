using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.Universal.Controls
{
    [TemplatePart(Name = CountText, Type = typeof(TextBlock))]
    public sealed class UnplayedCountControl : Control
    {
        private const string CountText = "CountText";
        private const string LayoutRoot = "LayoutRoot";
        private TextBlock _countText;
        private Grid _layoutRoot;

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
            "Item", typeof(BaseItemDto), typeof(UnplayedCountControl), new PropertyMetadata(default(BaseItemDto), OnItemChanged));

        public BaseItemDto Item
        {
            get { return (BaseItemDto)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public UnplayedCountControl()
        {
            DefaultStyleKey = typeof(UnplayedCountControl);
        }

        protected override void OnApplyTemplate()
        {
            _countText = GetTemplateChild(CountText) as TextBlock;
            _layoutRoot = GetTemplateChild(LayoutRoot) as Grid;
            Update();

            base.OnApplyTemplate();
        }

        private static void OnItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as UnplayedCountControl)?.Update();
        }

        private void Update()
        {
            if (!ViewModelBase.IsInDesignModeStatic && (Item == null || _countText == null || _layoutRoot == null || ItemDoesNotShowACount()))
            {
                return;
            }

            if (!ViewModelBase.IsInDesignModeStatic)
            {
                _countText.Text = Item?.UserData?.UnplayedItemCount?.ToString();
                _layoutRoot.Visibility = Visibility.Visible;
            }
        }

        private bool ItemDoesNotShowACount()
        {
            var type = Item.Type.ToLower();
            var validTypes = new[] { "series", "season", "boxset", "video", "musicalbum", "album", "photoalbum", "artist", "musicartist" };

            return !validTypes.Contains(type);
        }
    }
}
