using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Emby.Mobile.Universal.Services
{
    public static class TitleBarService
    {
        private static readonly Color? EmbyColor = (Color?)Application.Current.Resources["EmbyGreenColor"];

        public static void SetTitleBarColours()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (titleBar != null)
            {
                titleBar.BackgroundColor = EmbyColor;
                titleBar.ForegroundColor = GetColourFromResources("TitleBarForegroundColor");
                titleBar.ButtonBackgroundColor = EmbyColor;
                titleBar.ButtonForegroundColor = GetColourFromResources("TitleBarButtonForegroundColor");
                titleBar.ButtonHoverBackgroundColor = GetColourFromResources("TitleBarButtonHoverColor");
            }
        }

        private static Color? GetColourFromResources(string resource)
        {
            return (Color?) Application.Current.Resources[resource];
        }
    }
}
