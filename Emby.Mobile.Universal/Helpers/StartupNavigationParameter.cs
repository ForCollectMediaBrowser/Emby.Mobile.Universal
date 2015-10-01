using Windows.ApplicationModel.Activation;

namespace Emby.Mobile.Universal.Helpers
{
    public class StartupNavigationParameter
    {
        public object Args { get; set; }
        public SplashScreen SplashScreen { get; set; }

        public StartupNavigationParameter(object args, SplashScreen splashScreen)
        {
            Args = args;
            SplashScreen = splashScreen;
        }
    }
}
