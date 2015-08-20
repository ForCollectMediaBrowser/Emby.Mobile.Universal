using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Core.NullServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MediaBrowser.Model.ApiClient;
using Microsoft.Practices.ServiceLocation;
using ScottIsAFool.Windows.MvvmLight.Extensions;

namespace Emby.Mobile.Universal.Services
{
    public static class AppServices
    {
        public static void Create()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                AddDesignTimeServices();
            }
            else
            {
                // Create run time view services and models
                AddRuntimeServices();
            }
        }

        private static void AddRuntimeServices()
        {
            SimpleIoc.Default.RegisterIf<INavigationService, NavigationService>();
        }

        private static void AddDesignTimeServices()
        {
            SimpleIoc.Default.RegisterIf<INavigationService, NullNavigationService>();
            SimpleIoc.Default.RegisterIf<IConnectionManager, NullConnectionManager>();
        }

        public static INavigationService NavigationService => ServiceLocator.Current.GetInstance<INavigationService>();
    }
}
