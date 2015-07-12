using Cimbalino.Toolkit.Services;
using Emby.Universal.Core.NullServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ScottIsAFool.Windows.MvvmLight.Extensions;

namespace Emby.Universal.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                SimpleIoc.Default.RegisterIf<INavigationService, NullNavigationService>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.RegisterIf<INavigationService, NavigationService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public static INavigationService NavigationService => ServiceLocator.Current.GetInstance<INavigationService>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}