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
            IStatusBarService statusBar)
        {
            NavigationService = navigationService;
            MessageBox = messageBox;
            Launcher = launcher;
            StatusBar = statusBar;
        }

        public INavigationService NavigationService { get; }
        public ILauncherService Launcher { get; }
        public IMessageBoxService MessageBox { get; }
        public IStatusBarService StatusBar { get; }
    }
}