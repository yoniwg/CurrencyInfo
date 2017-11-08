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
            return String.Format("{0:F2}%",value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string strValue = value.ToString();
            return decimal.TryParse(strValue.Remove(strValue.Length - 1), out decimal dec) ? dec as decimal? : null;
        }
    }
}
