using System.Diagnostics;
using Windows.Storage;

namespace Emby.Mobile.Universal.BackgroundAudio
{
    public static class BackgroundAudioCommunicationSettings
    {
        public const string TrackUri = "trackuri";
        public const string Title = "title";
        public const string AlbumArt = "albumart";
        public const string TrackId = "trackid";
        public const string Position = "position";
        public const string BackgroundAudioTaskState = "backgroundaudiotaskstate"; // Started, Running, Cancelled
        public const string AppState = "appstate"; // Suspended, Resumed
        public const string AppSuspendedTimestamp = "appsuspendedtimestamp";
        public const string AppResumedTimestamp = "appresumedtimestamp";

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
