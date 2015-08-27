using System.Threading.Tasks;
using Emby.Mobile.Core.Helpers;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Helpers;
using MediaBrowser.Model.Connect;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.ViewModels
{
    public class ConnectPinEntryViewModel : PageViewModelBase
    {
        private PinCreationResult _pinInfo;
        private Timer _timer; 


        public ConnectPinEntryViewModel(IServices services) : base(services)
        {
        }

        public string Pin { get; set; }

        protected override async Task PageLoaded()
        {
            try
            {
                _pinInfo = await Services.ConnectionManager.CreatePin();
                Pin = _pinInfo.Pin;

                _timer = CreateTimer();
            }
            catch (HttpException ex)
            {

            }
            finally
            {
                
            }
        }

        private Timer CreateTimer()
        {
            return new Timer(CheckPin, null, 3000, 3000);
        }

        private async void CheckPin(object state)
        {
            try
            {
                var pinInfo = await Services.ConnectionManager.GetPinStatus(_pinInfo);
                if (pinInfo.IsConfirmed)
                {
                    _timer.Dispose();

                    await Services.ConnectionManager.ExchangePin(_pinInfo);
                    var result = await Services.ConnectionManager.Connect();

                    await ConnectHelper.HandleConnectState(result, Services, ApiClient);
                }
                else if (pinInfo.IsExpired)
                {
                    _timer.Dispose();
                }
            }
            catch (HttpException ex)
            {

            }
            finally
            {
                
            }
        }
    }
}
