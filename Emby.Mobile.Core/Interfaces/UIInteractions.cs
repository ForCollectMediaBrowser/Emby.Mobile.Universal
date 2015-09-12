using Cimbalino.Toolkit.Services;

namespace Emby.Mobile.Core.Interfaces
{
    // ReSharper disable once InconsistentNaming
    public interface IUIInteractions
    {
        INavigationService NavigationService { get; }
        IMessageBoxService MessageBox { get; }
        ILauncherService Launcher { get; }
        IStatusBarService StatusBar { get; }
    }
}
