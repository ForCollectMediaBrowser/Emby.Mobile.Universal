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
using Emby.Mobile.Universal.Core.Helpers;

namespace Emby.Mobile.ViewModels
{
    public class ChooseServerViewModel : PageViewModelBase
    {
        private bool _serversLoaded;

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

        public RelayCommand RefreshCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await LoadData(true);
                });
            }
        }

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

        public RelayCommand SignOutCommand => new RelayCommand(async () => { await SignOutHelper.SignOut(Services); });

        protected override async Task PageLoaded()
        {
            await LoadData(false);
        }

        private async Task LoadData(bool isRefresh)
        {
            if (_serversLoaded && !isRefresh)
            {
                return;
            }

            SetProgressBar(Resources.SysTrayGettingServers);

            try
            {
                var connect = await Services.ConnectionManager.Connect();
                var servers = connect.Servers;

                if (servers.IsNullOrEmpty())
                {
                    return;
                }

                Servers = servers.Select(x => new ServerInfoViewModel(Services, x)).ToObservableCollection();
                Servers.Add(new ServerInfoViewModel(Services, null) {IsDummyServer = true});

                _serversLoaded = !Servers.IsNullOrEmpty();
            }
            catch (HttpException ex)
            {

            }
            finally
            {
                SetProgressBar();
            }
        }
    }
}
