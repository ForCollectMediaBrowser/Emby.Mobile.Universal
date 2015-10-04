using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Strings;
using Emby.Mobile.Helpers;
using Emby.Mobile.Messages;
using Emby.Mobile.ViewModels.Entities;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.ViewModels.Connect
{
    public abstract class ChooseServerBaseViewModel : PageViewModelBase
    {
        private bool _serversLoaded;

        protected ChooseServerBaseViewModel(IServices services) : base(services)
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
                    }),
                    new ServerInfoViewModel(services, null)
                    {
                        IsDummyServer = true
                    }
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
                    Services.UiInteractions.NavigationService.NavigateToManualServerEntry();
                });
            }
        }

        public RelayCommand SignOutCommand => new RelayCommand(async () => { await SignOutHelper.SignOut(Services); });

        protected override Task PageLoaded()
        {
            return LoadData(false);
        }

        public override Task Refresh()
        {
            return LoadData(true);
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

        protected async Task LoadData(bool isRefresh)
        {
            if (_serversLoaded && !isRefresh)
            {
                return;
            }

            SetProgressBar(Resources.SysTrayFindingServer);

            try
            {
                var servers = await GetServers();

                if (servers.IsNullOrEmpty())
                {
                    return;
                }

                Servers = servers.Select(x => new ServerInfoViewModel(Services, x)).ToObservableCollection();
                Servers.Add(new ServerInfoViewModel(Services, null) { IsDummyServer = true });

                _serversLoaded = !Servers.IsNullOrEmpty();
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

        protected virtual Task<List<ServerInfo>> GetServers()
        {
            return Task.FromResult(new List<ServerInfo>());
        }
    }
}