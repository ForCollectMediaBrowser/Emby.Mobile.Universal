using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Core.Helpers;
using GalaSoft.MvvmLight.Command;

namespace Emby.Mobile.ViewModels
{
    public class MainViewModel : PageViewModelBase
    {
        public MainViewModel(IServices services) : base(services)
        {
        }

        public string ConnectedTo => string.Format(Core.Strings.Resources.LabelServerConnected, Services.ServerInfo?.ServerInfo?.Name);

        public RelayCommand SignOutCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await SignOutHelper.SignOut(Services);
                });
            }
        }
    }
}
