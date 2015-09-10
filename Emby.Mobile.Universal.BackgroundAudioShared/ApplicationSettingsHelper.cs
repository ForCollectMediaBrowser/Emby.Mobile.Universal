using System.Diagnostics;
using Windows.Storage;

namespace Emby.Mobile.Universal.BackgroundAudio
{
    public static class ApplicationSettingsHelper
    {
        public static object ReadAndRemoveSettingsValue(string key)
        {
            Debug.WriteLine(key);
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {                
                return null;
            }
            else
            {
                var value = ApplicationData.Current.LocalSettings.Values[key];
                ApplicationData.Current.LocalSettings.Values.Remove(key);             
                return value;
            }
        }

        public static void SaveSettingsValue(string key, object value)
        {          
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                ApplicationData.Current.LocalSettings.Values.Add(key, value);
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values[key] = value;
            }
        }
    }
}
