using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Messages;
using GalaSoft.MvvmLight.Messaging;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Logging;

namespace Emby.Mobile.ViewModels
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        protected IServices Services { get; }
        protected ILogger Log { get; }

        protected virtual bool UseSystemForProgress { get; }

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

        protected void ShowStatusBarWarning(string text)
        {            
            Services.UiInteractions.StatusBar.DisplayWarning(text);            
        }

        protected void ShowStatusBarError(string text)
        {
            Services.UiInteractions.StatusBar.DisplayError(text);
        }

        protected void ShowStatusBarMessage(string text)
        {
            Services.UiInteractions.StatusBar.DisplayMessage(text);
        }

        protected void SetProgressBar(string text)
        {
            ProgressIsVisible = true;
            ProgressText = text;
            if (UseSystemForProgress)
            {
                Services.UiInteractions.StatusBar.DisplayIndeterminateStatus(text);
            }

            UpdateProperties();
        }

        protected void SetProgressBar()
        {
            ProgressIsVisible = false;
            ProgressText = string.Empty;
            if (UseSystemForProgress)
            {
                Services.UiInteractions.StatusBar.DisplayIndeterminateStatus(string.Empty);
            }

            UpdateProperties();
        }       

        protected virtual Task OnSignOut()
        {
            return Task.FromResult(0);
        }

        protected virtual void UpdateProperties() { }

        public virtual Task Refresh()
        {
            return Task.FromResult(0);
        }

        public bool ProgressIsVisible { get; set; }
        public string ProgressText { get; set; }
        
        protected IApiClient ApiClient => Services.ServerInteractions.ConnectionManager.GetApiClient(Services.ServerInteractions.ServerInfo.ServerInfo?.Id);
        protected IAuthenticationService AuthenticationService => Services.ServerInteractions.Authentication;
        protected IAnalyticsService Analytics => Services.Analytics;
    }
}