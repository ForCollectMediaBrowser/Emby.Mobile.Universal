using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.System.Profile;
using Emby.Mobile.Universal.Core.Implementations.Security;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Devices;

namespace Emby.Mobile.Universal.Core.Implementations
{
    public class Device : IDevice
    {
        public string DeviceId { get; }

        public string DeviceName { get; }

        public event EventHandler<EventArgs> ResumeFromSleep;

        public Device()
        {
            DeviceId = GetDeviceId();
            DeviceName = GetDeviceName();
        }

        public async Task<IEnumerable<LocalFileInfo>> GetLocalPhotos()
        {
            var folder = KnownFolders.PicturesLibrary;

            var pictures = await folder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByDate);

            return from picture in pictures
                   select new LocalFileInfo
                   {
                       Id = picture.Path,
                       Name = picture.Name,
                       Album = string.Empty,
                       MimeType = GetMimeType(picture)
                   };
        }

        public async Task<IEnumerable<LocalFileInfo>> GetLocalVideos()
        {
            return new List<LocalFileInfo>();
        }

        public async Task UploadFile(LocalFileInfo file, IApiClient apiClient, CancellationToken cancellationToken)
        {
            var folder = KnownFolders.PicturesLibrary;

            var picture = await folder.GetFileAsync(file.Name);

            if (picture != null)
            {
                using (var stream = await picture.OpenReadAsync())
                {
                    await apiClient.UploadFile(stream.AsStream(), file, cancellationToken);
                }
            }
        }

        private static string GetMimeType(IStorageFile x)
        {
            var ext = x.FileType;
            return "image/" + (string.IsNullOrEmpty(ext) ? "jpeg" : ext.Replace(".", string.Empty));
        }

        private static string GetDeviceId()
        {
            var token = HardwareIdentification.GetPackageSpecificToken(null);
            var stream = token.Id.AsStream();
            using (var reader = new BinaryReader(stream))
            {
                var bytes = reader.ReadBytes((int)stream.Length);
                return new Guid(new CryptographyProvider().CreateMD5(bytes)).ToString();
            }
        }

        private static string GetDeviceName()
        {
            var hostNames = NetworkInformation.GetHostNames();

            var localName = hostNames.FirstOrDefault(name => name.DisplayName.Contains(".local"));

            if (localName != null)
            {
                var computerName = localName.DisplayName.Replace(".local", "");
                return computerName;
            }

            return "EmbyDefault";
        }
    }
}