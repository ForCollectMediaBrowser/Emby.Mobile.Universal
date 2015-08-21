using System;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Helpers;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.ViewModels
{
    public class EmbyConnectViewModel : ViewModelBase
    {
        public EmbyConnectViewModel(IServices services) 
            : base(services)
        {
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public bool CanSignIn => !ProgressIsVisible
                                 && !string.IsNullOrWhiteSpace(Username)
                                 && !string.IsNullOrWhiteSpace(Password);

        public RelayCommand SignInCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        await Services.ConnectionManager.LoginToConnect(Username, Password);

                        var result = await Services.ConnectionManager.Connect();
                        if (result.State == ConnectionState.SignedIn && result.Servers.Count == 1)
                        {
                            Services.ServerInfo.SetServerInfo(result.Servers[0]);
                            Services.ApplicationSettings.Roaming.Set(ConnectHelper.DefaultServerConnection, result.Servers[0]);
                        }

                        await ConnectHelper.HandleConnectState(result, Services);
                    }
                    catch (HttpException hex)
                    {

                    }
                    catch (Exception ex)
                    {
                        
                    }
                });
            }
        }

        public override void UpdateProperties()
        {
            RaisePropertyChanged(() => CanSignIn);
        }
    }
}
