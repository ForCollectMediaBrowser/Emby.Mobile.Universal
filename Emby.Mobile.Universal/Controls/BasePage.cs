using System.Linq;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Cimbalino.Toolkit.Services;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Core.Logging;
using Emby.Mobile.Universal.Helpers;
using Emby.Mobile.Universal.Interfaces;
using INavigationService = Cimbalino.Toolkit.Services.INavigationService;

namespace Emby.Mobile.Universal.Controls
{
    public abstract class BasePage : Page
    {
        protected readonly ILog Logger;
        
        protected virtual ApplicationViewBoundsMode Mode => ApplicationViewBoundsMode.UseVisible;

        public abstract INavigationService NavigationService { get; }

        protected virtual NavigationCacheMode NavCacheMode => NavigationCacheMode.Required;

        protected BasePage()
        {
            Logger = new WinLogger(GetType().FullName);
            Loaded += (sender, args) => NavigationCacheMode = NavCacheMode;
        }

        protected virtual void OnBackKeyPressed(object sender, NavigationServiceBackKeyPressedEventArgs e)
        {
        }

        protected void SetFullScreen()
        {
            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(Mode);
        }

        protected virtual void InitialiseOnBack()
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationService != null)
            {
                NavigationService.BackKeyPressed += OnBackKeyPressed;
            }

            Logger.Info("Navigated to {0}", GetType().FullName);

            SetFullScreen();

            if (e.NavigationMode == NavigationMode.Back)
            {
                InitialiseOnBack();
            }

            var parameters = e.Parameter as NavigationParameters;
            if (parameters != null)
            {
                if (parameters.ClearBackstack)
                {
                    Logger.Info("Clearing backstack");
                    Frame.BackStack.Clear();
                }
            }

            var homeVm = DataContext as ICanHasHomeButton;
            if (homeVm != null)
            {
                homeVm.ShowHomeButton = parameters != null && parameters.ShowHomeButton;
            }

            var navigationVm = DataContext as INavigatedToViewModel;
            navigationVm?.NavigatedTo(e);

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (NavigationService != null)
            {
                NavigationService.BackKeyPressed -= OnBackKeyPressed;
            }

            if (e.NavigationMode != NavigationMode.Back)
            {
                var parameters = e.Parameter as NavigationParameters;
                if (parameters != null && parameters.RemoveCurrentPageFromBackstack)
                {
                    var page = Frame.BackStack.FirstOrDefault(x => x.SourcePageType == GetType());
                    if (page != null)
                    {
                        Frame.BackStack.Remove(page);
                    }
                }
            }

            var navigationVm = DataContext as INavigatedFromViewModel;
            navigationVm?.NavigatedFrom(e);

            Logger.Info("Navigated from {0}", GetType().FullName);
            base.OnNavigatedFrom(e);
        }
    }
}
