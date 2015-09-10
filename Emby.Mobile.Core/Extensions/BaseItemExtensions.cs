using System;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Library;

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
                case "boxsets":
                case "folders":
                    icon = "\uE2C7";
                    break;
                default:
                    icon = string.Empty;
                    break;
            }

            return icon;
        }

        public static bool CanStream(this BaseItemDto item, UserDto user)
        {
            var canStream = false;

            if (user.Policy.EnableMediaPlayback)
            {
                if (IsPlayable(item))
                {
                    canStream = true;
                }
            }

            return canStream;
        }

        private static bool IsPlayable(BaseItemDto item)
        {
            return item != null 
                && (item.LocationType != LocationType.Virtual && item.Type != "Program") // This line might not be right *shrugs*
                   && (item.IsVideo || item.IsAudio)
                   && item.PlayAccess == PlayAccess.Full
                   && IsValidProgram(item);
        }

        private static bool IsValidProgram(BaseItemDto programme)
        {
            if (programme.Type != "Program")
            {
                // If not a TV programme then we don't care, so just return true.
                return true;
            }

            var now = DateTime.Now;
            return programme.StartDate.HasValue && programme.EndDate.HasValue && programme.StartDate.Value.ToLocalTime() < now && programme.EndDate.Value.ToLocalTime() > now;
        }
    }
}
