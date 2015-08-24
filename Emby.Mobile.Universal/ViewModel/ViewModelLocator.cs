using Emby.Mobile.Universal.Services;
using Emby.Mobile.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Emby.Mobile.Universal.ViewModel
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

            AppServices.Create();

            SimpleIoc.Default.Register<EmbyConnectViewModel>();
            SimpleIoc.Default.Register<StartupViewModel>();
        }

        public StartupViewModel Startup => ServiceLocator.Current.GetInstance<StartupViewModel>();
        public EmbyConnectViewModel EmbyConnect => ServiceLocator.Current.GetInstance<EmbyConnectViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}