using Emby.Mobile.Universal.Services;
using Emby.Mobile.ViewModels;
using Emby.Mobile.ViewModels.Connect;
using Emby.Mobile.ViewModels.UserViews;
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
            Register<ConnectChooseServerViewModel>();
            Register<ChooseServerViewModel>();
            Register<ChooseUserProfileViewModel>();
            Register<MainViewModel>();
            Register<ManualServerEntryViewModel>();
            Register<ManualLocalUserSignInViewModel>();
            Register<ConnectPinEntryViewModel>();
            Register<HeaderMenuViewModel>();
            Register<SearchViewModel>();
            Register<GenericItemViewModel>();
            Register<MovieViewModel>();
            Register<TvShowViewModel>();
            Register<SeasonViewModel>();
            Register<SettingsViewModel>();
            Register<MovieUserViewModel>();
            Register<TvUserViewModel>();
        }

        public MainViewModel Main => Get<MainViewModel>();
        public StartupViewModel Startup => Get<StartupViewModel>();
        public ConnectViewModel Connect => Get<ConnectViewModel>();
        public ConnectChooseServerViewModel ConnectChooseServer => Get<ConnectChooseServerViewModel>();
        public ChooseServerViewModel ChooseServer => Get<ChooseServerViewModel>();
        public ChooseUserProfileViewModel ChooseUserProfile => Get<ChooseUserProfileViewModel>();
        public ManualServerEntryViewModel ManualServerEntry => Get<ManualServerEntryViewModel>();
        public ConnectSignUpViewModel ConnectSignUp => Get<ConnectSignUpViewModel>();
        public ManualLocalUserSignInViewModel ManualLocalUser => Get<ManualLocalUserSignInViewModel>();
        public ConnectPinEntryViewModel PinEntry => Get<ConnectPinEntryViewModel>();
        public HeaderMenuViewModel Header => Get<HeaderMenuViewModel>();
        public SearchViewModel Search => Get<SearchViewModel>();
        public GenericItemViewModel Generic => Get<GenericItemViewModel>();
        public MovieViewModel Movie => Get<MovieViewModel>();
        public TvShowViewModel TvShow => Get<TvShowViewModel>();
        public SeasonViewModel Season => Get<SeasonViewModel>();
        public SettingsViewModel Settings => Get<SettingsViewModel>();
        public MovieUserViewModel MovieUserView => Get<MovieUserViewModel>();
        public TvUserViewModel TvUserView => Get<TvUserViewModel>();
        
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

        internal static T Get<T>(string id)
        {
            return ServiceLocator.Current.GetInstance<T>(id);
        }
    }
}