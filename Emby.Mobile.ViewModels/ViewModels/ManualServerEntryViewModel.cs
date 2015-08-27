using System;
using System.Linq;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Helpers;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Net;
using Emby.Mobile.Core.Strings;

namespace Emby.Mobile.ViewModels
{
    public class ManualServerEntryViewModel : PageViewModelBase
    {
        public ManualServerEntryViewModel(IServices services) : base(services)
        {
            PortNumber = 8096;
        }

        public string ServerAddress { get; set; }
        public int PortNumber { get; set; }
        public bool IsHttps { get; set; }

        public bool CanConnect => !string.IsNullOrEmpty(ServerAddress)
                                  && (PortNumber >= 0 && PortNumber <= 65535)
                                  && !ProgressIsVisible;

        public string DisplayUrl
        {
            get
            {
                if (string.IsNullOrEmpty(ServerAddress)) return string.Empty;

                var uriBuilder = new UriBuilder(IsHttps ? "https" : "http", ServerAddress, PortNumber);
                return uriBuilder.Uri.ToString();
            }
        }

        public RelayCommand ConnectCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var success = false;
                    try
                    {
                        SetProgressBar(Resources.SysTrayConnecting);

                        var result = await Services.ConnectionManager.Connect(DisplayUrl);

                        if (result.State != ConnectionState.Unavailable)
                        {
                            success = true;
                            var server = result.Servers.FirstOrDefault();
                            if (server != null)
                            {
                                Services.ServerInfo.SetServerInfo(server);
                            }

                            await ConnectHelper.HandleConnectState(result, Services, ApiClient);
                        }
                    }
                    catch (HttpException ex)
                    {
                        Log.ErrorException("ConnectCommand", ex);
                    }
                    finally
                    {
                        if (!success)
                        {
                            await Services.MessageBox.ShowAsync("ErrorUnableToConnect");
                        }

                        SetProgressBar();
                    }
                });
            }
        }

        public RelayCommand TestUrlCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (Uri.IsWellFormedUriString(DisplayUrl, UriKind.Absolute))
                    {
                        Services.Launcher.LaunchUriAsync(DisplayUrl);
                    }
                });
            }
        }

        protected override void UpdateProperties()
        {
            RaisePropertyChanged(() => CanConnect);
        }
    }
}
