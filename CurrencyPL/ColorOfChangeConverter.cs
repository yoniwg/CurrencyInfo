using System;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace CurrencyPL
{
    public class ColorOfChangeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var redColor = System.Drawing.Color.Red;
            var greenColor = System.Drawing.Color.Green;
            var yellowColor = System.Drawing.Color.Yellow;
            var blackColor = System.Drawing.Color.Black;
            decimal? decValue = value as decimal?;
            if (!decValue.HasValue)
            {

                return blackColor;
            }
            var desiredColor = (decValue > 0) ? greenColor : (decValue < 0) ? redColor : yellowColor;
            //return Color.FromArgb(desiredColor.A, desiredColor.R, desiredColor.G, desiredColor.B);
            return (decValue > 0) ? "Green" : (decValue < 0) ? "Red" : "Yellow";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}