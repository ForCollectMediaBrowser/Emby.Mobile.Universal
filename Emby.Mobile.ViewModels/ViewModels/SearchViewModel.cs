using System.Collections.ObjectModel;
using Cimbalino.Toolkit.Extensions;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.Net;
using MediaBrowser.Model.Search;
using Emby.Mobile.Core.Strings;
using Emby.Mobile.ViewModels.Entities;
using System.Linq;

namespace Emby.Mobile.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        public SearchViewModel(IServices services) : base(services)
        {
            if (!IsInDesignMode)
            {
                SearchResults = new ObservableCollection<SearchHintViewModel>();
            }
            else
            {
                SearchResults = new ObservableCollection<SearchHintViewModel>
                {
                    new SearchHintViewModel(services, new SearchHint { Name = "Result 1" }),
                    new SearchHintViewModel(services,new SearchHint { Name = "Result 2" })
                };
            }
        }

        public string SearchText { get; set; }
        public ObservableCollection<SearchHintViewModel> SearchResults { get; set; }

        private async void OnSearchTextChanged()
        {
            if (SearchText?.Length > 1)
            {
                var query = new SearchQuery
                {
                    UserId = AuthenticationService.SignedInUserId,
                    Limit = 20,
                    StartIndex = 0,
                    IncludeArtists = true,
                    IncludeGenres = true,
                    IncludeMedia = true,
                    IncludePeople = true,
                    IncludeStudios = true,
                    SearchTerm = SearchText
                };

                try
                {
                    SetProgressBar(Resources.SysTraySearching);
                    SearchResults.Clear();
                    var search = await ApiClient.GetSearchHintsAsync(query);
                    if (search != null && !search.SearchHints.IsNullOrEmpty())
                    {
                        SearchResults = search.SearchHints.Select(x => new SearchHintViewModel(Services, x)).ToObservableCollection();
                        
                    }
                }
                catch (HttpException e)
                {

                }
                finally
                {
                    SetProgressBar();
                }
            }
        }
    }
}
