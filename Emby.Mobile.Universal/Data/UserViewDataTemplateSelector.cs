using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Emby.Mobile.ViewModels.Entities;

namespace Emby.Mobile.Universal.Data
{
    public class UserViewDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate MovieTemplate { get; set; }
        public DataTemplate ChannelTemplate { get; set; }
        public DataTemplate LiveTvTemplate { get; set; }
        public DataTemplate TvTemplate { get; set; }
        public DataTemplate MusicTemplate { get; set; }
        public DataTemplate PlaylistsTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var card = item as UserViewViewModel;
            if (card == null)
            {
                return DefaultTemplate;
            }

            DataTemplate template;
            switch (card.CollectionType.ToLower())
            {
                case "movie":
                    template = MovieTemplate;
                    break;
                case "tvshows":
                    template = TvTemplate;
                    break;
                case "music":
                    template = MusicTemplate;
                    break;
                case "channels":
                    template = ChannelTemplate;
                    break;
                case "livetv":
                    template = LiveTvTemplate;
                    break;
                case "playlists":
                    template = PlaylistsTemplate;
                    break;
                case "photos":
                case "guide":
                case "boxsets":
                case "folders":
                default:
                    template = DefaultTemplate;
                    break;
            }

            return template;
        }
    }
}
