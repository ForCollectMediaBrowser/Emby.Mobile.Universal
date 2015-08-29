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

        public RelayCommand SkipCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Cancel();
                    Services.NavigationService.NavigateToManualServerEntry();
                });
            }
        } 

        public RelayCommand PinAddressCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Services.Launcher.LaunchUriAsync("http://emby.media/pin");
                });
            }
        }

        public void Cancel()
        {
            if (PinHelper.IsBusy)
            {
                PinHelper.Cancel();
            }
        }

        protected override async Task PageLoaded()
        {
            try
            {
                if (PinHelper.IsBusy)
                {
                    return;
                }

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

                        AuthenticationService.SetConnectUser(connectResult.ConnectUser);

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
