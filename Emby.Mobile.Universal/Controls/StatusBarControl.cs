using Emby.Mobile.Universal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Controls
{
    [TemplateVisualState(GroupName = "CommonStates", Name = "Open")]
    [TemplateVisualState(GroupName = "CommonStates", Name = "Closed")]
    [TemplateVisualState(GroupName = "DisplayStates", Name = "Error")]
    [TemplateVisualState(GroupName = "DisplayStates", Name = "Normal")]
    [TemplateVisualState(GroupName = "DisplayStates", Name = "Success")]
    [TemplateVisualState(GroupName = "DisplayStates", Name = "Message")]
    [TemplateVisualState(GroupName = "DisplayStates", Name = "Warning")]
    public sealed class StatusBarControl : Control
    {
        private static StatusBarControl _currentControl;

        private List<DisplayItem> _items = new List<DisplayItem>();
        private DisplayItem _currentItem;

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(StatusBarControl), new PropertyMetadata(""));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public StatusBarControl()
        {
            DefaultStyleKey = typeof(StatusBarControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, "Closed", true);
            RegisterControlForView(this);
        }

        public void Show(string text, StatusType type)
        {
            var item = new DisplayItem(text, type);

            _items.RemoveAll(i => i.Type == type);

            _items.Add(item);

            ShowInternal();
        }

        private async void ShowInternal()
        {
            var item = GetNextItem();

            if (item == null || string.IsNullOrEmpty(item.Text))
            {
                if (_currentItem == null)
                {
                    VisualStateManager.GoToState(this, "Closed", true);
                }
                return;
            }
            VisualStateManager.GoToState(this, "Open", true);

            _currentItem = item;
            Text = item.Text;

            SetDisplayState(item);

            await HandleNextState(item);
        }

        private void SetDisplayState(DisplayItem item)
        {
            switch (item.Type)
            {
                case StatusType.Error:
                    VisualStateManager.GoToState(this, "Error", true);
                    break;
                case StatusType.Success:
                    VisualStateManager.GoToState(this, "Success", true);
                    break;
                case StatusType.Warning:
                    VisualStateManager.GoToState(this, "Warning", true);
                    break;
                case StatusType.Message:
                    VisualStateManager.GoToState(this, "Message", true);
                    break;
                default:
                    VisualStateManager.GoToState(this, "Normal", true);
                    break;
            }
        }

        private async Task HandleNextState(DisplayItem item)
        {
            switch (item.Type)
            {
                case StatusType.Message:
                case StatusType.Success:
                case StatusType.Warning:
                case StatusType.Error:
                    _currentItem = null;
                    await Task.Delay(item.Type == StatusType.Error ? 7000 : 4000);
                    ShowInternal();
                    break;
            }
        }

        private DisplayItem GetNextItem()
        {
            var item = _items.OrderBy(c => c.CreatedTime).FirstOrDefault();
            if (item != null)
            {
                _items.Remove(item);
            }
            return item;
        }

        public static StatusBarControl GetForCurrentView()
        {
            return _currentControl;
        }

        private static void RegisterControlForView(StatusBarControl control)
        {
            _currentControl = control;
        }

        private class DisplayItem
        {
            public DateTime CreatedTime { get; }
            public string Text { get; }
            public StatusType Type { get; }

            public DisplayItem(string text, StatusType type)
            {
                Text = text;
                Type = type;
                CreatedTime = DateTime.UtcNow;
            }
        }

    }
}
