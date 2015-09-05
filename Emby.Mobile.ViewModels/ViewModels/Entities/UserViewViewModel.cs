using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Strings;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Net;
using MediaBrowser.Model.Querying;

namespace Emby.Mobile.ViewModels.Entities
{
    [DebuggerDisplay("Name: {Name}, Type: {CollectionType}")]
    public class UserViewViewModel : ItemViewModel
    {
        private bool _cardLoaded;

        public UserViewViewModel(IServices services, BaseItemDto itemInfo) : base(services, itemInfo)
        {
            GetCardData(false).ConfigureAwait(false);
        }

        protected override bool UseSystemForProgress { get; } = false;

        public ObservableCollection<ItemViewModel> Items { get; set; }

        public string CollectionType => ItemInfo?.CollectionType;

        private async Task GetCardData(bool isRefresh)
        {
            if (_cardLoaded && !isRefresh)
            {
                return;
            }

            try
            {
                SetProgressBar(Resources.LabelLoading);

                var query = new ItemQuery
                {
                    ParentId = ItemInfo.Id,
                    UserId = AuthenticationService.SignedInUserId,
                    SortBy = new[] { "DateCreated" },
                    SortOrder = SortOrder.Descending,
                    //IncludeItemTypes = new[] { "Movie" },
                    Limit = 3,
                    Fields = new[] { ItemFields.PrimaryImageAspectRatio },
                    Filters = new[] { ItemFilter.IsUnplayed },
                    Recursive = true,
                    ImageTypeLimit = 1,
                    EnableImageTypes = new[] { ImageType.Backdrop, ImageType.Primary, }
                };

                var response = await ApiClient.GetItemsAsync(query);
                if (response != null && !response.Items.IsNullOrEmpty())
                {
                    var items = response.Items.Select(x => new ItemViewModel(Services, x)).ToObservableCollection();

                    Items = items;
                    _cardLoaded = Items.Any();
                }
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
