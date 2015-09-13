using Emby.Mobile.ViewModels;
using Emby.Mobile.ViewModels.Connect;

namespace Emby.Mobile.Universal.Views.Connect
{
    public sealed partial class ChooseUserProfileView
    {
        public ChooseUserProfileViewModel ChooseUser  => DataContext as ChooseUserProfileViewModel;
        
        public ChooseUserProfileView()
        {
            this.InitializeComponent();
        }
    }
}
