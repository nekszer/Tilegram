using System;
using Windows.UI.Xaml.Data;

namespace Tilegram.Converters
{

    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
                return dateTime.ToString("MMMM d, yyyy");
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
