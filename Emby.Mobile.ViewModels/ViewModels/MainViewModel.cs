using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Strings;
using Emby.Mobile.Universal.Core.Helpers;
using Emby.Mobile.ViewModels.Entities;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.ViewModels
{
    public class MainViewModel : PageViewModelBase
    {
        private bool _viewsLoaded;

        public MainViewModel(IServices services) : base(services)
        {
            UserViews = new ObservableCollection<UserViewViewModel>();
        }

        public string ConnectedTo => string.Format(Resources.LabelServerConnected, Services.ServerInfo?.ServerInfo?.Name);

        public ObservableCollection<UserViewViewModel> UserViews { get; set; }

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

        protected override Task PageLoaded()
        {
            return LoadData(false);
        }

        protected override Task Refresh()
        {
            return LoadData(true);
        }

        private async Task LoadData(bool isRefresh)
        {
            if (_viewsLoaded && !isRefresh)
            {
                return;
            }

            try
            {
                var item = await ApiClient.GetUserViews(AuthenticationService.SignedInUserId);
                if (item != null && !item.Items.IsNullOrEmpty())
                {
                    UserViews.Clear();
                    var items = item.Items.Select(x => new UserViewViewModel(Services, x)).ToObservableCollection();

                    UserViews = items;
                    _viewsLoaded = UserViews.Any();
                }
            }
            catch (HttpException ex)
            {
                //Utils.HandleHttpException("GetUserViews()", ex, NavigationService, Log);
            }
        }
    }
}
