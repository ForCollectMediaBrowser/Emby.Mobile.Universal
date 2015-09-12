using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Emby.Mobile.ViewModels
{
    public class SettingsViewModel : PageViewModelBase
    {
        public SettingsViewModel(IServices services) : base(services)
        {
        }

        protected override Task PageLoaded()
        {
            GetDeviceName();
            return base.PageLoaded();
        }

        #region Change Device name
        public string DeviceName { get; set; }

        public RelayCommand SaveDeviceNameCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (string.IsNullOrEmpty(DeviceName))
                    {
                        // TODO: Display error message
                        return;
                    }

                    var deviceId = Services.Device.Device.DeviceId;
                    if (Services.Settings.DeviceNames.ContainsKey(deviceId))
                    {
                        Services.Settings.DeviceNames[deviceId] = DeviceName;
                    }
                    else
                    {
                        Services.Settings.DeviceNames.Add(deviceId, DeviceName);
                    }
                });
            }
        }

        private void GetDeviceName()
        {
            var deviceId = Services.Device.Device.DeviceId;
            DeviceName = Services.Settings.DeviceNames.ContainsKey(deviceId)
                ? Services.Settings.DeviceNames[deviceId]
                : Services.Device.Device.DeviceName;
        }

        #endregion
    }
}
