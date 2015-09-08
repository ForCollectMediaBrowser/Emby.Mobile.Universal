using Emby.Mobile.Helpers;

namespace Emby.Mobile.Universal.Extensions
{
    public static class NavigationModeExtensions
    {
        public static NavigationMode ExchangeMode(this Windows.UI.Xaml.Navigation.NavigationMode mode)
        {
            var result = NavigationMode.New;

            switch (mode)
            {
                case Windows.UI.Xaml.Navigation.NavigationMode.Back:
                    result = NavigationMode.Back;
                    break;
                case Windows.UI.Xaml.Navigation.NavigationMode.Forward:
                    result = NavigationMode.Forward;
                    break;
                case Windows.UI.Xaml.Navigation.NavigationMode.New:
                    result = NavigationMode.New;
                    break;
                case Windows.UI.Xaml.Navigation.NavigationMode.Refresh:
                    result = NavigationMode.Refresh;
                    break;
            }

            return result;
        }
    }
}
