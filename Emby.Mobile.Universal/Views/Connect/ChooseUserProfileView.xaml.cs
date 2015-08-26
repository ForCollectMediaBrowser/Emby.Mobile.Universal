using Emby.Mobile.ViewModels;

namespace Emby.Mobile.Universal.Views.Connect
{
    public sealed partial class ChooseUserProfileView
    {
        public ChooseUserProfileViewModel Vm
        { get { return DataContext as ChooseUserProfileViewModel; } }
        public ChooseUserProfileView()
        {
            this.InitializeComponent();
        }
    }
}
