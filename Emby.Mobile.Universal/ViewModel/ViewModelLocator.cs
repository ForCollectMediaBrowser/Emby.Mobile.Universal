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

            Register<ConnectViewModel>();
            Register<ConnectSignUpViewModel>();
            Register<StartupViewModel>();
            Register<ChooseServerViewModel>();
            Register<ChooseUserProfileViewModel>();
            Register<MainViewModel>();
            Register<ManualServerEntryViewModel>();
            Register<ManualLocalUserSignInViewModel>();
            Register<ConnectPinEntryViewModel>();
            Register<BurgerMenuViewModel>();
        }

        public MainViewModel Main => Get<MainViewModel>();
        public StartupViewModel Startup => Get<StartupViewModel>();
        public ConnectViewModel Connect => Get<ConnectViewModel>();
        public ChooseServerViewModel ChooseServer => Get<ChooseServerViewModel>();
        public ChooseUserProfileViewModel ChooseUserProfile => Get<ChooseUserProfileViewModel>();
        public ManualServerEntryViewModel ManualServerEntry => Get<ManualServerEntryViewModel>();
        public ConnectSignUpViewModel ConnectSignUp => Get<ConnectSignUpViewModel>();
        public ManualLocalUserSignInViewModel ManualLocalUser => Get<ManualLocalUserSignInViewModel>();
        public ConnectPinEntryViewModel PinEntry => Get<ConnectPinEntryViewModel>();
        public BurgerMenuViewModel Burger => Get<BurgerMenuViewModel>();



        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        private static void Register<T>(bool createImmediately = false)
            where T : class
        {
            SimpleIoc.Default.Register<T>(createImmediately);
        }

        internal static T Get<T>() 
            where T : class
        {
            return ServiceLocator.Current.GetInstance<T>();
        }
    }
}