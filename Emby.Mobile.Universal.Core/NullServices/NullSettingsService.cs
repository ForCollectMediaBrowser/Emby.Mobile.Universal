using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullSettingsService : ISettingsService
    {
        public bool ShowMissingEpisodes { get; set; }
        public bool ShowUnairedEpisodes { get; set; }
        public bool EnableImageEnhancers { get; set; }
        public ISettingsService Load()
        {
            return null;
        }

        public void Save()
        {
        }
    }
}
