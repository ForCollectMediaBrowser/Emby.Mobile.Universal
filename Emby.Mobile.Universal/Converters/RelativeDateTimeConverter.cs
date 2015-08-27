using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Strings;
using System;
using Windows.UI.Xaml.Data;

namespace Emby.Mobile.Universal.Converters
{
    public class RelativeDateTimeConverter : IValueConverter
    {
        private static ILocalizedResources _resources;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (_resources == null)
                _resources = new LocalizedResources();

            var dateTime = value as DateTime?;

            if(dateTime.HasValue)
            {
                const int SECOND = 1;
                const int MINUTE = 60 * SECOND;
                const int HOUR = 60 * MINUTE;
                const int DAY = 24 * HOUR;
                const int MONTH = 30 * DAY;
                
                var date = dateTime.Value;
                int offset = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                date = date.AddHours(offset);

                var ts = DateTime.UtcNow.Subtract(date);
                double seconds = ts.TotalSeconds;
                
                // Less than one minute
                if (seconds < 1 * MINUTE)
                    return ts.Seconds < 2 ? _resources.GetString("LabelOneSecondAgo") : String.Format(_resources.GetString("LabelSecondsAgo"), ts.Seconds);

                if (seconds < 60 * MINUTE)
                    return ts.Minutes == 1 ? _resources.GetString("LabelOneMinuteAgo") : String.Format(_resources.GetString("LabelMinutesAgo"), ts.Minutes);

                if (seconds < 120 * MINUTE)
                    return _resources.GetString("LabelAnHourAgo");

                if (seconds < 24 * HOUR)
                    return String.Format(_resources.GetString("LabelHoursAgo"), ts.Hours);

                if (seconds < 48 * HOUR)
                    return _resources.GetString("LabelYesterday");

                if (seconds < 30 * DAY)
                    return String.Format(_resources.GetString("LabelDaysAgo"), ts.Days);

                if (seconds < 12 * MONTH)
                {
                    int months =  System.Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    return months <= 1 ? _resources.GetString("LabelOneMonthAgo") : String.Format(_resources.GetString("LabelMonthsAgo"), months);
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}