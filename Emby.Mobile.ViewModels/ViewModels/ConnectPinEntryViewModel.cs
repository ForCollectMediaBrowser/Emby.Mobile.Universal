using System;
using System.Threading.Tasks;
using Emby.Mobile.Core.Helpers;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Helpers;
using GalaSoft.MvvmLight.Command;

namespace Emby.Mobile.ViewModels
{
    public class ConnectPinEntryViewModel : PageViewModelBase
    {
        public ConnectPinEntryViewModel(IServices services) : base(services)
        {
        }

        public string Pin { get; set; }

        public RelayCommand CancelCommand => new RelayCommand(PinHelper.Cancel);

        protected override async Task PageLoaded()
        {
            try
            {
                SetProgressBar("Getting pin...");
                var result = await PinHelper.ConnectUsingPin(Services, SetPin);

                switch (result)
                {
                    case PinResult.Cancelled:
                    case PinResult.Expired:
                    case PinResult.Fail:
                    case PinResult.Unknown:

                        break;
                    case PinResult.Success:
                        var connectResult = await Services.ConnectionManager.Connect();

                        await ConnectHelper.HandleConnectState(connectResult, Services, ApiClient);
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                SetProgressBar();
            }
        }

        private void SetPin(string pin)
        {
            Services.Dispatcher.RunAsync(() =>
            {
                Pin = pin;
            });
        }
    }
}
