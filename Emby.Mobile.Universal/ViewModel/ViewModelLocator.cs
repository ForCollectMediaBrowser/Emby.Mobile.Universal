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
            SimpleIoc.Default.Register<ChooseServerViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ManualServerEntryViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public StartupViewModel Startup => ServiceLocator.Current.GetInstance<StartupViewModel>();
        public EmbyConnectViewModel EmbyConnect => ServiceLocator.Current.GetInstance<EmbyConnectViewModel>();
        public ChooseServerViewModel ChooseServer => ServiceLocator.Current.GetInstance<ChooseServerViewModel>();
        public ManualServerEntryViewModel ManualServerEntry => ServiceLocator.Current.GetInstance<ManualServerEntryViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}