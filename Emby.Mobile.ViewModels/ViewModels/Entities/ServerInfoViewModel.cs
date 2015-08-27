using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Helpers;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.ViewModels.Entities
{
    public class ServerInfoViewModel : ViewModelBase
    {
        public ServerInfoViewModel(IServices services, ServerInfo serverInfo) : base(services)
        {
            ServerInfo = serverInfo;
        }

        public ServerInfo ServerInfo { get; set; }

        public string ServerName => ServerInfo?.Name;

        public string LocalAddress => ServerInfo?.LocalAddress;

        public string ExternalAddress => ServerInfo?.RemoteAddress;

        public bool DisplayLocalAddress => !string.IsNullOrEmpty(LocalAddress);

        public bool DisplayExternalAddress => !string.IsNullOrEmpty(ExternalAddress);

        public bool IsDummyServer { get; set; }

        public RelayCommand ServerTappedCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    SetProgressBar(Core.Strings.Resources.SysTrayConnecting);

                    var result = await Services.ConnectionManager.Connect(ServerInfo);

                    if (result.State == ConnectionState.Unavailable)
                    {
                        Log.Info("Invalid connection details");
                        await Services.MessageBox.ShowAsync(Core.Strings.Resources.ErrorUnableToConnect);
                    }
                    else
                    {
                        Services.Messenger.SendAppResetNotification();
                        AuthenticationService.ClearLoggedInUser();
                        SaveServer(ServerInfo);
                        await ConnectHelper.HandleConnectState(result, Services, ApiClient);
                    }

                    SetProgressBar();
                });
            }
        }

        private void SaveServer(ServerInfo server)
        {
            Services.ServerInfo.SetServerInfo(server);
            Services.ServerInfo.Save();
        }
    }
}
