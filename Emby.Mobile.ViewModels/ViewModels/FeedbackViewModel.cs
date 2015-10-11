using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Emby.Mobile.ViewModels
{
    public class FeedbackViewModel : ViewModelBase
    {
        private const string EmailTo = "wpmb3@outlook.com"; // We may need to change this :)

        public FeedbackViewModel(IServices services) : base(services)
        {
        }

        public string FeedbackText { get; set; }

        public RelayCommand SendFeedbackCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (string.IsNullOrEmpty(FeedbackText))
                    {
                        return;
                    }

                    Services.UiInteractions.Email.ShowAsync(EmailTo, "Feedback from Emby Universal", FeedbackText);

                    FeedbackText = string.Empty;
                });
            }
        }

        public RelayCommand ClearFeedbackCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    FeedbackText = string.Empty;
                });
            }
        }
    }
}
