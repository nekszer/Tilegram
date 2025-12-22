using System;
using Windows.UI.Xaml.Data;

namespace Tilegram.Converters
{
    public class CommentsCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string countText)
                return $"View all {countText} comments";
            return "View comments";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
