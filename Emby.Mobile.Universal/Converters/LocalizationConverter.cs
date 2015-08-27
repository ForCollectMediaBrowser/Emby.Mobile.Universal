using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Strings;
using System;
using Windows.UI.Xaml.Data;

namespace Emby.Mobile.Universal.Converters
{
    public class LocalizationConverter : IValueConverter
    {
        private static ILocalizedResources _resources;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (_resources == null)
                _resources = new LocalizedStrings();
            return _resources.GetString(parameter?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}