using System;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace CurrencyPL
{
    public class ColorOfChangeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            decimal? decValue = value as decimal?;
            if (!decValue.HasValue)
            {
                return "Black";
            }
            return (decValue > 0) ? "Green" : (decValue < 0) ? "Red" : "Yellow";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}