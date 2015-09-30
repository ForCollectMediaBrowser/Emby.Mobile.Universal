using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;
using Cimbalino.Toolkit.Helpers;
using Emby.Mobile.Universal.Core.Implementations.Connection;
using Emby.Mobile.Universal.Core.Implementations.Security;
using MediaBrowser.ApiInteraction;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Dlna;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Session;
using MediaBrowser.Streaming;

namespace Emby.Mobile.Universal.Core.Helpers
{
    public static class ConnectionManagerFactory
    {
        const string ProfileFilename = "StreamingProfile.xml";

        public async static Task<IConnectionManager> CreateConnectionManager(
            IDevice device,
            ILogger logger,
            INetworkConnection networkConnection,
            ICredentialProvider credentialProvider)
        {
            var manager = new ConnectionManager(
                logger,
                credentialProvider,
                networkConnection,
                new ServerLocator(),
                "Windows Universal",
                ApplicationVersion,
                device,
                await GetDeviceCapabilitiesAsync(),
                new CryptographyProvider(),
                () => new WebSocketClient());

            return manager;
        }

        public static string ApplicationVersion
        {
            get
            {
                var version = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        private static async Task<ClientCapabilities> GetDeviceCapabilitiesAsync()
        {
            return new ClientCapabilities
            {
                SupportedCommands = new List<string>
                {
                    "ToggleOsd",
                    "GoHome",
                    "VolumeUp",
                    "VolumeDown",
                    "Mute",
                    "Unmute",
                    "ToggleMute",
                    "SetVolume",
                    "SetAudioStreamIndex",
                    "SetSubtitleStreamIndex",
                    "Back",

                },
                PlayableMediaTypes = new List<string>
                {
                    "Audio",
                    "Video",
                },
                SupportsContentUploading = true,
                SupportsSync = true,
                SupportsMediaControl = true,
                SupportsOfflineAccess = true,
                DeviceProfile = await GetProfileAsync(),
                AppStoreUrl = "http://apps.microsoft.com/windows/app/media-browser/ad55a2f0-9897-47bd-8944-bed3aefd5d06",
                IconUrl = "https://raw.githubusercontent.com/MediaBrowser/Emby.Mobile.Universal/feature/Home_View_Tweaks/Windows10Logo.png" // TODO: This will need changing to dev URL when merged.
            };
        }

        public static async Task<DeviceProfile> GetProfileAsync()
        {
            return new Windows10Profile();
            //TODO Enable this when we get further on down the road.
            DeviceProfile profile = null;

            try
            {
                var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync(ProfileFilename);

                var file = item as StorageFile;

                if (file != null)
                {
                    using (IInputStream inStream = await file.OpenSequentialReadAsync())
                    {
                        var serializer = new XmlSerializer(typeof(DeviceProfile));
                        profile = (DeviceProfile)serializer.Deserialize(inStream.AsStreamForRead());
                        return profile;
                    }

                    if (profile == null)
                    {
                        throw new SerializationException();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                //TODO App.Logger.Info("StreamingProfile doesn't exist, Creating new default");
            }
            //catch (Exception e)
            //{
            //    //TODO App.Logger.Error("Failed to serialize profile, will use default", e);
            //    return new Windows81HlsProfile();
            //}

            return await CreateDefaultProfile();
        }

        private static async Task<DeviceProfile> CreateDefaultProfile()
        {
            try
            {
                var profile = new Windows10Profile();
                MemoryStream streamProfile = new MemoryStream();
                var serializer = new XmlSerializer(profile.GetType());
                serializer.Serialize(streamProfile, profile);

                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(ProfileFilename, CreationCollisionOption.ReplaceExisting);
                using (Stream fileStream = await file.OpenStreamForWriteAsync())
                {
                    streamProfile.Seek(0, SeekOrigin.Begin);
                    await streamProfile.CopyToAsync(fileStream);
                }
            }
            catch (Exception e)
            {
                //TODO App.Logger.Error("Failed to write DefaultProfile", e);
            }

            return new Windows10Profile();
        }
    }
}
