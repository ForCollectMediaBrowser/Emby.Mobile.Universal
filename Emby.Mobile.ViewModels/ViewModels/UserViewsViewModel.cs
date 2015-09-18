using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.ViewModels.Entities;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.ViewModels
{
    public class UserViewsViewModel : ViewModelBase
    {
        private bool _viewsLoaded;

        public UserViewsViewModel(IServices services) : base(services)
        {
            UserViews = new ObservableCollection<UserViewViewModel>();
        }

        public override Task Refresh()
        {
            return LoadData(true);
        }

        public ObservableCollection<UserViewViewModel> UserViews { get; set; }

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
