using Emby.Mobile.Core.Interfaces;
using Windows.ApplicationModel.Resources;

namespace Emby.Mobile.Universal.Strings
{
    internal class LocalizedResources : ILocalizedResources
    {
        private ResourceLoader _resources;
        public LocalizedResources()
        {
            _resources = ResourceLoader.GetForViewIndependentUse();
        }

        public string GetString(string key)
        {
            return _resources.GetString(key);
        }

    }
}
