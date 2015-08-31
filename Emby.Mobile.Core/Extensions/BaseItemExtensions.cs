using MediaBrowser.Model.Dto;

namespace Emby.Mobile.Core.Extensions
{
    public static class BaseItemExtensions
    {
        public static string GetMaterialIcon(this BaseItemDto item)
        {
            if (string.IsNullOrEmpty(item?.CollectionType))
            {
                return string.Empty;
            }

            var viewType = item.CollectionType.ToLower();
            string icon;
            switch (viewType)
            {
                case "boxsets":
                case "recordedtv":
                case "movies":
                    icon = "\uE04A";
                    break;
                case "tvshows":
                    icon = "\uE333";
                    break;
                case "music":
                    icon = "\uE030";
                    break;
                case "channels":
                    icon = "\uE04B";
                    break;
                case "livetv":
                    icon = "\uE639";
                    break;
                case "playlists":
                    icon = "\uE8EF";
                    break;
                case "photos":
                    icon = "\uE3B6";
                    break;
                case "guide":
                    icon = "\uE1B2";
                    break;
                case "folder":
                    icon = "\uE2C7";
                    break;
                default:
                    icon = string.Empty;
                    break;
            }

            return icon;
        }
    }
}
