using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace CurrencyPL
{
    class RatioStringFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            decimal? decValue = value as decimal?;
            if (!decValue.HasValue) return "Not Decimal";
            decimal percentValue = new decimal((double) (decValue * 100 - 100));
            if (percentValue < new decimal(0.01) && percentValue > new decimal(-0.01)) return "  --  ";
            return String.Format((decValue > 1) ? "+{0:F2}%" : "{0:F2}%", decValue * 100 - 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string strValue = value.ToString();
            return decimal.TryParse(strValue.Remove(strValue.Length - 1), out decimal dec) ? dec as decimal? : 0;
        }
    }
}
