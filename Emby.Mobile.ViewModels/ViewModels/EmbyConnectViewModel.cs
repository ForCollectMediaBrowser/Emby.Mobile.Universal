﻿using System;
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
                        SetProgressBar(GetLocalizedString("SysTraySigningIn"));

                        var success = await AuthenticationService.LoginWithConnect(Username, Password);

                        if (success)
                        {
                            var result = await Services.ConnectionManager.Connect();
                            if (result.State == ConnectionState.SignedIn && result.Servers.Count == 1)
                            {
                                Services.ServerInfo.SetServerInfo(result.Servers[0]);
                                Services.ServerInfo.Save();
                            }

                            AuthenticationService.SetConnectUser(result.ConnectUser);

                            await ConnectHelper.HandleConnectState(result, Services, ApiClient);

                            Services.NavigationService.RemoveBackEntry();
                        }
                    }
                    catch (HttpException hex)
                    {

                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        SetProgressBar();
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
