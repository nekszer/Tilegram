using System;
using Windows.UI.Xaml.Data;

namespace Light.UWP.MVVM.Converters
{
    public class ValueToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter == null)
                return value?.ToString() ?? string.Empty;

            var strFormat = parameter?.ToString() ?? string.Empty;
            if(string.IsNullOrEmpty(strFormat))
                return value?.ToString() ?? string.Empty;

            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
