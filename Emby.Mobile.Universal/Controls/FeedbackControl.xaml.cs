using Emby.Mobile.ViewModels;

namespace Emby.Mobile.Universal.Controls
{
    public sealed partial class FeedbackControl
    {
        private FeedbackViewModel Feedback => DataContext as FeedbackViewModel;

        public FeedbackControl()
        {
            InitializeComponent();
        }
    }
}
