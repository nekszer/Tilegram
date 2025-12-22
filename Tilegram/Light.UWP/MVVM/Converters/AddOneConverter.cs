using System;
using Windows.UI.Xaml.Data;

namespace Light.UWP.MVVM.Converters
{
    public class AddOneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int intValue)
            {
                return (intValue + 1).ToString();
            }

            if (value is long longValue)
            {
                return (longValue + 1).ToString();
            }

            // Si no es un número, intentar parsear
            if (value != null && int.TryParse(value.ToString(), out int parsed))
            {
                return (parsed + 1).ToString();
            }

            return "1";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
