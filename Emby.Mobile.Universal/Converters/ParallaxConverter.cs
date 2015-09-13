using System;
using Windows.UI.Xaml.Data;

namespace Emby.Mobile.Universal.Converters
{
    public class ParallaxConverter : IValueConverter
    {
        const double _factor = -0.5;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double)
            {
                return (double)value * _factor;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is double)
            {
                return (double)value / _factor;
            }
            return 0;
        }
    }

}
