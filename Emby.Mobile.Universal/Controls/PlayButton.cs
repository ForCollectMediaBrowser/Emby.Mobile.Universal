using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Emby.Mobile.Universal.Controls
{
    [TemplatePart(Name = ContainingGrid, Type = typeof(Grid))]
    public sealed class PlayButton : Control
    {
        private const string ContainingGrid = "ContainingGrid";
        private Grid _containingGrid;

        public static readonly DependencyProperty PlayCommandProperty = DependencyProperty.Register(
            "PlayCommand", typeof (ICommand), typeof (PlayButton), new PropertyMetadata(default(ICommand)));

        public ICommand PlayCommand
        {
            get { return (ICommand) GetValue(PlayCommandProperty); }
            set { SetValue(PlayCommandProperty, value); }
        }

        public static readonly DependencyProperty ResumeCommandProperty = DependencyProperty.Register(
            "ResumeCommand", typeof (ICommand), typeof (PlayButton), new PropertyMetadata(default(ICommand)));

        public ICommand ResumeCommand
        {
            get { return (ICommand) GetValue(ResumeCommandProperty); }
            set { SetValue(ResumeCommandProperty, value); }
        }

        public static readonly DependencyProperty CanResumeProperty = DependencyProperty.Register(
            "CanResume", typeof (bool), typeof (PlayButton), new PropertyMetadata(default(bool)));

        public bool CanResume
        {
            get { return (bool) GetValue(CanResumeProperty); }
            set { SetValue(CanResumeProperty, value); }
        }

        public PlayButton()
        {
            DefaultStyleKey = typeof(PlayButton);
            Tapped += OnTapped;
        }

        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (CanResume && _containingGrid != null)
            {
                FlyoutBase.ShowAttachedFlyout(_containingGrid);
            }
            else
            {
                PlayCommand?.Execute(null);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _containingGrid = GetTemplateChild(ContainingGrid) as Grid;
        }
    }
}
