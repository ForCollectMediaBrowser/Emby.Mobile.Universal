using Emby.Mobile.Core.Interfaces;
using System.Reflection;
using System.Resources;

namespace Emby.Mobile.Core.Strings
{
    public class LocalizedStrings : ILocalizedResources
    {
        private static readonly Resources _localisedResources = new Resources();
        private static ResourceManager _resourceManager;

        public Resources LocalisedResources => _localisedResources;

        public string GetString(string key)
        {
            if (_resourceManager == null)
                _resourceManager = new ResourceManager("Emby.Mobile.Core.Strings.Resources", typeof(Resources).GetTypeInfo().Assembly);
            return _resourceManager.GetString(key) ?? string.Empty;
        }
    }
}
