using Windows.System.Profile;
using Emby.Mobile.Core.Helpers;
using Emby.Mobile.Core.Interfaces;
using Windows.Foundation.Metadata;
using Cimbalino.Toolkit.Services;
using Emby.Mobile.Universal.Core.Implementations;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Universal.Core.Services
{
    public class DeviceInfoService : IDeviceInfoService
    {
        private readonly IDisplayPropertiesService _displayProperties;

        public DeviceInfoService(IDevice device, IDisplayPropertiesService displayProperties)
        {
            _displayProperties = displayProperties;
            Device = device;
            DeviceFamily = GetDeviceFamily();
        }

        public bool SupportsBackButton => ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
        public bool SupportsVibrate => ApiInformation.IsTypePresent("Windows.Phone.Devices.Notification.VibrationDevice");
        public bool SupportsStatusBar => ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar");

        public IDevice Device { get; }

        public void SetName(string name)
        {
            var device = Device as Device;
            if (device != null)
            {
                device.DeviceName = name;
            }
        }

        public int? GetDeviceScaleImageValue(int? value)
        {
            var result = value;

            if (value.HasValue)
            {
                var scale = _displayProperties.LogicalDpi;

                result = (int) (result*(scale/100));
            }

            return result;
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
