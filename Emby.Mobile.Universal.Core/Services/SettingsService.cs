using Cimbalino.Toolkit.Services;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Universal.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private const string SettingsServiceKey = "SettingsServiceKey";
        private readonly IApplicationSettingsServiceHandler _roamingSettings;

        public SettingsService(IApplicationSettingsService settings)
        {
            _roamingSettings = settings.Roaming;
        }

        public bool ShowMissingEpisodes { get; set; }
        public bool ShowUnairedEpisodes { get; set; }
        public bool EnableImageEnhancers { get; set; }
        public ISettingsService Load()
        {
            var settings = _roamingSettings.SafeGet<SettingsService>(SettingsServiceKey);
            return settings;
        }

        public void Save()
        {
            _roamingSettings.SafeSet(SettingsServiceKey, this);
        }
    }
}
