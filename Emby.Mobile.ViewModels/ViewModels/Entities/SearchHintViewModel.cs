using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
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
                var options = new ImageOptions
                {
                    Quality = 70,
                    MaxWidth = 50,
                    ImageType = ImageType.Primary,
                    Format = MediaBrowser.Model.Drawing.ImageFormat.Png,
                };                
                ImageUrl = ApiClient?.GetImageUrl(searchHint.ItemId, options);
            }           
        }

        public SearchHint SearchHint { get; set; }

        public string ImageUrl { get; } = string.Empty;

        public string Name => SearchHint?.Name;

        public string Type => SearchHint?.Type; //TODO Translations needed for Types.
        
        public RelayCommand SearchHintTappedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {                    
                    switch(SearchHint?.Type)
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
