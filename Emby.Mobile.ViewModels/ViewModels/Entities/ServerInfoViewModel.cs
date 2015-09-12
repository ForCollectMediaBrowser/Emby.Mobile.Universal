using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Strings;
using Emby.Mobile.Helpers;
using Emby.Mobile.Messages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
                    SetProgressBar(Resources.SysTrayConnecting);

                    var result = await Services.ServerInteractions.ConnectionManager.Connect(ServerInfo);

                    if (result.State == ConnectionState.Unavailable)
                    {
                        Log.Info("Invalid connection details");
                        await Services.UiInteractions.MessageBox.ShowAsync(Resources.ErrorUnableToConnect);
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

        public RelayCommand RemoveServerCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var credsProvider = AuthenticationService.Credential;
                    await credsProvider.RemoveServer(ServerInfo);

                    Messenger.Default.Send(new ServerInfoMessage(this, ServerInfoMessage.DeleteServerMsg));
                });
            }
        }

        private void SaveServer(ServerInfo server)
        {
            Services.ServerInteractions.ServerInfo.SetServerInfo(server);
            Services.ServerInteractions.ServerInfo.Save();
        }
    }
}
