using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.Connect;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.Core.Helpers
{
    public enum PinResult
    {
        Unknown,
        Success,
        Fail,
        Expired,
        Cancelled
    }

    public static class PinHelper
    {
        private static Timer _timer;
        private static PinCreationResult _pinInfo;
        private static TaskCompletionSource<PinResult> _tcs;
        private static CancellationTokenSource _cancellationToken;

        public static bool IsBusy { get; private set; }

        public static async Task<PinResult> ConnectUsingPin(IServices services, Action<string> pinAcquired)
        {
            _cancellationToken = new CancellationTokenSource();
            PinResult result;

            IsBusy = true;

            try
            {
                _pinInfo = await services.ConnectionManager.CreatePin();
                pinAcquired?.Invoke(_pinInfo.Pin);

                _tcs = new TaskCompletionSource<PinResult>();

                _timer = CreateTimer(services);

                result = await _tcs.Task;
            }
            catch (HttpException ex)
            {
                result = PinResult.Fail;
            }
            finally
            {
                IsBusy = false;
            }

            return result;
        }

        public static void Cancel()
        {
            _cancellationToken?.Cancel();
        }

        private static Timer CreateTimer(IServices services)
        {
            return new Timer(CheckPin, services, 3000, 3000, _cancellationToken.Token);
        }

        private static async void CheckPin(object state)
        {
            try
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    SetResult(PinResult.Cancelled);
                    return;
                }

                var services = state as IServices;
                if (services == null) return;

                var pinInfo = await services.ConnectionManager.GetPinStatus(_pinInfo);
                if (pinInfo.IsConfirmed)
                {
                    _timer.Dispose();

                    await services.ConnectionManager.ExchangePin(_pinInfo);
                    SetResult(PinResult.Success);
                }
                else if (pinInfo.IsExpired)
                {
                    SetResult(PinResult.Expired);
                }
            }
            catch (HttpException ex)
            {
                if (!ex.StatusCode.HasValue || ex.StatusCode.Value != HttpStatusCode.NotFound)
                {
                    SetResult(PinResult.Fail);
                }
            }
            catch (Exception)
            {
                SetResult(PinResult.Fail);
            }
        }

        private static void SetResult(PinResult result)
        {
            _timer.Dispose();
            _tcs.SetResult(result);
        }
    }
}