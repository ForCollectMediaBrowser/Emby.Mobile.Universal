using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Emby.Mobile.Universal.Triggers
{
    /// <summary>
    /// This class is purely for debugging purposes
    /// </summary>
    public class EmbyAdaptiveTrigger : AdaptiveTrigger
    {
        public EmbyAdaptiveTrigger()
        {
            var window = Window.Current;

            window.SizeChanged -= CurrentOnSizeChanged;
            window.SizeChanged += CurrentOnSizeChanged;
        }

        private void CurrentOnSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            var i = 1;
        }
    }
}
