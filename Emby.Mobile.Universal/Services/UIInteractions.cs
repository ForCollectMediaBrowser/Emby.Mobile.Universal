using Cimbalino.Toolkit.Services;
using Emby.Mobile.Core.Interfaces;
using INavigationService = Emby.Mobile.Core.Interfaces.INavigationService;
using IStatusBarService = Emby.Mobile.Core.Interfaces.IStatusBarService;

namespace Emby.Mobile.Universal.Services
{
    // ReSharper disable once InconsistentNaming
    public class UIInteractions : IUIInteractions
    {
        public UIInteractions(
            INavigationService navigationService,
            IMessageBoxService messageBox,
            ILauncherService launcher,
            IStatusBarService statusBar,
            IEmailComposeService email)
        {
            NavigationService = navigationService;
            MessageBox = messageBox;
            Launcher = launcher;
            StatusBar = statusBar;
            Email = email;
        }

        public INavigationService NavigationService { get; }
        public ILauncherService Launcher { get; }
        public IMessageBoxService MessageBox { get; }
        public IStatusBarService StatusBar { get; }
        public IEmailComposeService Email { get; }
    }
}