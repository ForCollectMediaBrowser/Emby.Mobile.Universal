using Windows.System.Profile;
using Emby.Mobile.Core.Helpers;
using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Universal.Core.Services
{
    public class DeviceInfoService : IDeviceInfoService
    {
        public DeviceInfoService()
        {
            DeviceFamily = GetDeviceFamily();
        }

        public DeviceFamily DeviceFamily { get; }

        private static DeviceFamily GetDeviceFamily()
        {
#if DEBUG
            return DeviceFamily.Xbox;
#else
            var deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            DeviceFamily result;

            switch (deviceFamily)
            {
                case "Windows.Mobile":
                    result = DeviceFamily.Mobile;
                    break;
                case "Windows.Desktop":
                    result = DeviceFamily.Desktop;
                    break;
                case "Windows.Team":
                    result = DeviceFamily.Team;
                    break;
                case "Windows.IoT":
                    result = DeviceFamily.IoT;
                    break;
                case "Windows.Xbox":
                    result = DeviceFamily.Xbox;
                    break;
                default:
                    result = DeviceFamily.Unknown;
                    break;
            }

            return result;
#endif
        }
    }
}
