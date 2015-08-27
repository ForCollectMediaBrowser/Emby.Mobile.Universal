using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Messages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Logging;

namespace Emby.Mobile.ViewModels
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        protected IServices Services { get; }
        protected ILogger Log { get; }

        protected ViewModelBase(IServices services)
        {
            Services = services;
            Log = Services.Log;

            if (!IsInDesignMode)
            {
                WireMessages();
            }
        }

        protected virtual void WireMessages()
        {
            Messenger.Default.Register<SignOutAppMessage>(this, async m =>
            {
                await OnSignOut();
            });
        }

        protected void SetProgressBar(string text)
        {
            ProgressIsVisible = true;
            ProgressText = text;

            UpdateProperties();
        }

        protected void SetProgressBar()
        {
            ProgressIsVisible = false;
            ProgressText = string.Empty;

            UpdateProperties();
        }       

        protected virtual Task OnSignOut()
        {
            return Task.FromResult(0);
        }

        protected virtual void UpdateProperties() { }
        
        public bool ProgressIsVisible { get; set; }
        public string ProgressText { get; set; }
        
        protected IApiClient ApiClient => Services.ConnectionManager.GetApiClient(Services?.ServerInfo?.ServerInfo?.Id);
        protected IAuthenticationService AuthenticationService => Services.Authentication;
    }

    public abstract class PageViewModelBase : ViewModelBase
    {
        protected PageViewModelBase(IServices services) : base(services)
        {
        }

        public RelayCommand PageLoadedCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await PageLoaded();
                });
            }
        }

        protected virtual Task PageLoaded()
        {
            return Task.FromResult(0);
        }
    }
}