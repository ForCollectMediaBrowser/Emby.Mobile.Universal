using System.Threading.Tasks;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Helpers;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.ViewModels
{
    public class StartupViewModel : PageViewModelBase
    {
        private ServerInfo _serverInfo;
        public StartupViewModel(IServices services) : base(services)
        {
        }

        protected override Task PageLoaded()
        {
            return LoadSettings();
        }

        public bool RetryButtonIsVisible { get; set; }

        public RelayCommand RetryConnectionCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await ConnectToServer();
                });
            }
        }

        private Task LoadSettings()
        {
            var task = Task.FromResult(0);
            RetryButtonIsVisible = false;
            // TODO: Check to see if OOBE has happened.

            // TODO: Load specific app settings

            // TODO: Load photo upload settings

            _serverInfo = Services.ServerInfo.Load();

            if (_serverInfo != null)
            {
                task = ConnectToServer();
            }
            else
            {
                // TODO: This should navigate to First run, but for now, can go to Connect sign in
                //Services.NavigationService.NavigateToFirstRun();
                Services.NavigationService.NavigateToEmbyConnect();
                Services.NavigationService.RemoveBackEntry();
            }

            SetProgressBar();

            return task;
        }

        private async Task<int> ConnectToServer()
        {
            RetryButtonIsVisible = false;
            ConnectionResult result = null;

            SetProgressBar(Services.LocalizedResources.GetString("SysTrayGettingServerDetails"));

            if (_serverInfo != null)
            {
                result = await Services.ConnectionManager.Connect(_serverInfo);
            }

            if (result != null && result.State == ConnectionState.Unavailable && _serverInfo != null)
            {
                RetryButtonIsVisible = true;
                return 0;
            }

            // See if we can find and communicate with the server

            if (result == null || result.State == ConnectionState.Unavailable)
            {
                result = await Services.ConnectionManager.Connect();
            }

            var tcs = new TaskCompletionSource<int>();
            await Services.Dispatcher.RunAsync(async () =>
            {
                await ConnectHelper.HandleConnectState(result, Services, ApiClient);

                SetProgressBar();

                tcs.SetResult(0);
            });

            await tcs.Task;

            return 0;
        }
    }
}
