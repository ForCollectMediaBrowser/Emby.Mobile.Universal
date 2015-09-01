using Windows.System.Profile;
using Emby.Mobile.Core.Helpers;
using Emby.Mobile.Core.Interfaces;
using Windows.Foundation.Metadata;

namespace Emby.Mobile.Universal.Core.Services
{
    public class DeviceInfoService : IDeviceInfoService
    {
        public bool SupportsBackButton => ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
        public bool SupportsVibrate => ApiInformation.IsTypePresent("Windows.Phone.Devices.Notification.VibrationDevice");
        public bool SupportsStatusBar => ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar");

        public DeviceInfoService()
        {
            DeviceFamily = GetDeviceFamily();
        }

        public DeviceFamily DeviceFamily { get; }

        private static DeviceFamily GetDeviceFamily()
        {
#if XBOX
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
