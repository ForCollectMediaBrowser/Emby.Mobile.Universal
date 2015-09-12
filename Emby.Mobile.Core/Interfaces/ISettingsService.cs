using System.Collections.Generic;

namespace Emby.Mobile.Core.Interfaces
{
    public interface ISettingsService
    {
        bool ShowMissingEpisodes { get; set; }
        bool ShowUnairedEpisodes { get; set; }
        bool EnableImageEnhancers { get; set; }
        Dictionary<string, string> DeviceNames { get; set; }
        ISettingsService Load();
        void Save();
    }
}
