using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.ViewModels.Entities;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Net;
using Emby.Mobile.Core.Strings;
using Emby.Mobile.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace Emby.Mobile.ViewModels
{
    public class ChooseServerViewModel : PageViewModelBase
    {
        public ChooseServerViewModel(IServices services) : base(services)
        {
            if (IsInDesignMode)
            {
                Servers = new ObservableCollection<ServerInfoViewModel>
                {
                    new ServerInfoViewModel(services, new ServerInfo
                    {
                        Name = "Ferret-Server",
                        LocalAddress = "http://192.168.0.27",
                        RemoteAddress = "http://scottisafool.server.com"
                    }),
                    new ServerInfoViewModel(services, new ServerInfo
                    {
                        Name = "7-Server",
                        LocalAddress = "http://192.168.0.2",
                        RemoteAddress = "http://7illusions.server.com"
                    })
                };
            }
        }

        public ObservableCollection<ServerInfoViewModel> Servers { get; set; }

        public RelayCommand ManualServerEntryCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Services.NavigationService.NavigateToManualServerEntry();
                });
            }
        }

        protected override async Task PageLoaded()
        {
            await LoadData();
        }

        protected override void WireMessages()
        {
            Messenger.Default.Register<ServerInfoMessage>(this, m =>
            {
                if (m.Notification.Equals(ServerInfoMessage.DeleteServerMsg))
                {
                    if (!Servers.IsNullOrEmpty())
                    {
                        Servers.Remove(m.Server);
                    }
                }
            });

            base.WireMessages();
        }

        private async Task LoadData()
        {
            SetProgressBar(Resources.SysTrayFindingServer);

            try
            {
                var servers = await Services.ConnectionManager.GetAvailableServers();

                if (servers.IsNullOrEmpty())
                {
                    return;
                }

                Servers = servers.Select(x => new ServerInfoViewModel(Services, x)).ToObservableCollection();
                Servers.Add(new ServerInfoViewModel(Services, null) { IsDummyServer = true });
            }
            catch (HttpException ex)
            {

            }
            finally
            {
                if (Servers?.Any() != true)
                {
                    ShowStatusBarWarning(Resources.ErrorCouldNotFindServer);
                }
                else
                {
                    SetProgressBar();
                }
            }
        }
    }
}