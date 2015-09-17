using System;
using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels.Entities
{
    public class MusicTrackViewModel : ItemViewModel
    {
        public MusicTrackViewModel(IServices services, BaseItemDto itemInfo) : base(services, itemInfo)
        {
        }

        public string TrackRuntime => CreateRuntime(ItemInfo);
        public bool DisplayRuntime => !string.IsNullOrEmpty(TrackRuntime);

        private static string CreateRuntime(BaseItemDto itemInfo)
        {
            if (itemInfo?.RunTimeTicks == null)
            {
                return string.Empty;
            }

            var ticks = itemInfo.RunTimeTicks.Value;
            var ts = TimeSpan.FromTicks(ticks);
            if (ts.Hours == 0)
            {
                return $"{ts.Minutes:00}:{ts.Seconds:00}";
            }

            return $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
        }
    }
}
