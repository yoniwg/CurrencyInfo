﻿using System;
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
            return (decValue > 1) ? "Green" : (decValue < 1) ? "Red" : "Black";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}