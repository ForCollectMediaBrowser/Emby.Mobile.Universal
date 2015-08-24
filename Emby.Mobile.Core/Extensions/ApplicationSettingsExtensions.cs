using Cimbalino.Toolkit.Services;
using Newtonsoft.Json;

namespace Emby.Mobile.Core.Extensions
{
    public static class ApplicationSettingsExtensions
    {
        public static void SafeSet<T>(this IApplicationSettingsServiceHandler settings, string key, T item)
        {
            var json = JsonConvert.SerializeObject(item);
            settings.Set(key, json);
        }

        public static T SafeGet<T>(this IApplicationSettingsServiceHandler settings, string key, T defaultValue = default(T)) where T : class
        {
            var json = settings.Get<string>(key);
            return string.IsNullOrEmpty(json) ? defaultValue : (JsonConvert.DeserializeObject<T>(json) ?? defaultValue);
        }
    }
}
