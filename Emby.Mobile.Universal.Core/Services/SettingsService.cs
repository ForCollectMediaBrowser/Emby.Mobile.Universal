using System.Collections.Generic;
using Cimbalino.Toolkit.Services;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Ioc;

namespace Emby.Mobile.Universal.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private const string SettingsServiceKey = "SettingsServiceKey";
        private readonly IApplicationSettingsServiceHandler _roamingSettings;

        public SettingsService() { }

        [PreferredConstructor]
        public SettingsService(IApplicationSettingsService settings)
        {
            _roamingSettings = settings.Roaming;
            DeviceNames = new Dictionary<string, string>();
        }

        public bool ShowMissingEpisodes { get; set; }
        public bool ShowUnairedEpisodes { get; set; }
        public bool EnableImageEnhancers { get; set; }
        public Dictionary<string, string> DeviceNames { get; set; }

        public ISettingsService Load()
        {
            var settings = _roamingSettings.SafeGet<SettingsService>(SettingsServiceKey);
            settings?.CopyItem(this);
            return this;
        }

        public void Save()
        {
            _roamingSettings.SafeSet(SettingsServiceKey, this);
        }
    }
}
