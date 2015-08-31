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
    [TemplateVisualState(GroupName = "DisplayStates", Name = "Warning")]
    [TemplatePart(Name = "DismissButton", Type = typeof(Button))]
    public sealed class SystemTrayControl : Control
    {
        private DispatcherTimer _progressTimer;
        private Button _dismissButton;
        private List<DisplayItem> items = new List<DisplayItem>();

        private DisplayItem currentItem;

        private double stepValue = 0.1d;

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SystemTrayControl), new PropertyMetadata(""));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty ShowProgressProperty = DependencyProperty.Register("ShowProgress", typeof(Visibility), typeof(SystemTrayControl), new PropertyMetadata(Visibility.Collapsed));
        public Visibility ShowProgress
        {
            get { return (Visibility)GetValue(ShowProgressProperty); }
            set { SetValue(ShowProgressProperty, value); }
        }

        public static readonly DependencyProperty CanDismissProperty = DependencyProperty.Register("CanDismiss", typeof(Visibility), typeof(SystemTrayControl), new PropertyMetadata(Visibility.Collapsed));
        public Visibility CanDismiss
        {
            get { return (Visibility)GetValue(CanDismissProperty); }
            set { SetValue(CanDismissProperty, value); }
        }

        public SystemTrayControl()
        {
            DefaultStyleKey = typeof(SystemTrayControl);
            _progressTimer = new DispatcherTimer();
            _progressTimer.Tick += progressTimer_Tick;
            _progressTimer.Interval = TimeSpan.FromMilliseconds(50);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._dismissButton = this.GetTemplateChild("DismissButton") as Button;
            _dismissButton.Click += dismissButton_Click;
            VisualStateManager.GoToState(this, "Closed", true);
            RegisterControlForView(this);
        }

        private void dismissButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentItem != null)
            {
                currentItem = null;
                DisplayProgessInternal();
            }
        }

        async void progressTimer_Tick(object sender, object e)
        {
            if (currentItem == null)
            {
                //This could happen if an item has been dismissed
                _progressTimer.Stop();
                return;
            }

            _progressTimer.Stop();

            await Task.Delay(currentItem.Type == StatusType.Error ? 7000 : 4000);
            currentItem = null;
            DisplayProgessInternal();
        }

        public void DisplayProgress(string text, StatusType type)
        {
            var item = new DisplayItem(text, type);
            items.RemoveAll(i => i.Type == type);
            items.Add(item);

            DisplayProgessInternal();
        }

        private void DisplayProgessInternal()
        {
            var item = GetNextItem();

            if (item == null || string.IsNullOrEmpty(item.Text))
            {
                if (currentItem == null)
                {
                    //All done, close the statuscontrol
                    VisualStateManager.GoToState(this, "Closed", true);
                }
                return;
            }

            VisualStateManager.GoToState(this, "Open", true);

            currentItem = item;
            Text = item.Text;
            ShowProgress = item.Type == StatusType.Status ? Visibility.Visible : Visibility.Collapsed;

            switch (currentItem.Type)
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
                default:
                    VisualStateManager.GoToState(this, "Normal", true);
                    break;
            }

            if (!_progressTimer.IsEnabled)
                _progressTimer.Start();
        }
        
        private DisplayItem GetNextItem()
        {
            var item = items.OrderBy(c => c.CreatedTime).FirstOrDefault();
            if (item != null)
            {
                items.Remove(item);
            }
            return item;
        }

        private static SystemTrayControl _currentControl;
        public static SystemTrayControl GetForCurrentView()
        {
            return _currentControl;
        }

        private static void RegisterControlForView(SystemTrayControl control)
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
