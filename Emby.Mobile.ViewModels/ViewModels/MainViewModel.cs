using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.ViewModels
{
    public class MainViewModel : PageViewModelBase
    {
        public MainViewModel(IServices services) : base(services)
        {
        }

        public string ConnectedTo => string.Format(GetLocalizedString("LabelServerConnected"), Services.ServerInfo?.ServerInfo?.Name);
    }
}
