using System;
using System.Threading.Tasks;
using Emby.Mobile.Core.Helpers;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Strings;
using Emby.Mobile.Helpers;
using GalaSoft.MvvmLight.Command;

namespace Emby.Mobile.ViewModels.Connect
{
    public class ConnectPinEntryViewModel : PageViewModelBase
    {
        public ConnectPinEntryViewModel(IServices services) : base(services)
        {
        }

        public string Pin { get; set; }

        public bool HasExpired { get; set; }

        public RelayCommand SkipCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Cancel();
                    Services.UiInteractions.NavigationService.NavigateToManualServerEntry();
                });
            }
        }

        public RelayCommand RetryCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await ConnectWithPin();
                });
            }
        }

        public RelayCommand PinAddressCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Services.UiInteractions.Launcher.LaunchUriAsync("http://emby.media/pin");
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
            await ConnectWithPin();
        }

        private async Task ConnectWithPin()
        {
            try
            {
                if (PinHelper.IsBusy)
                {
                    return;
                }

                HasExpired = false;

                SetProgressBar(Resources.SysTrayGettingPin);
                var result = await PinHelper.ConnectUsingPin(Services, SetPin);

                switch (result)
                {
                    case PinResult.Cancelled:
                    case PinResult.Fail:
                    case PinResult.Unknown:

                        break;

                    case PinResult.Expired:
                        HasExpired = true;
                        break;

                    case PinResult.Success:
                        var connectResult = await Services.ServerInteractions.ConnectionManager.Connect();

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
