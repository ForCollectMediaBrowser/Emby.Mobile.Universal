using Emby.Mobile.Core.Helpers;
using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Search;

namespace Emby.Mobile.ViewModels.Entities
{
    public class SearchHintViewModel : ViewModelBase
    {
        public SearchHintViewModel(IServices services, SearchHint searchHint) : base(services)
        {
            SearchHint = searchHint;
            if (!string.IsNullOrEmpty(SearchHint?.PrimaryImageTag))
            {
                ImageUrl = ApiClient?.GetImageUrl(searchHint.ItemId, ImageOptionsHelper.SearchHint);
            }
        }

        public SearchHint SearchHint { get; set; }

        public string ImageUrl { get; } = "ms-appx:///Assets/Tiles/150x150Logo.png";

        public string Name => SearchHint?.Name;

        public string Type => SearchHint?.Type; //TODO Translations needed for Types.

        public RelayCommand SearchHintTappedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    switch (SearchHint?.Type)
                    {
                        //TODO Navigate to TypeDetails-view;
                        default:
                            break;
                    }
                });
            }
        }
    }
}
