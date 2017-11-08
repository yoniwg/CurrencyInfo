using CurrencyBE;
using System;

namespace CurrencyPL.ViewModels
{
    public class AppPreferences 
    {

        private Currency mainTargetCurrency = new Currency("ILS");

        public Currency MainTargetCurrency {
            get => mainTargetCurrency;
            set => mainTargetCurrency = requireNotNull(value, "MainTargetCurrency");
        }

        private T requireNotNull<T>(T value, string propertyName)
        {
            if (value == null) throw new ArgumentNullException("trying to assign null into '" + propertyName + "' property.");
            return value;
        }

        private Currency defaultSourceCurrency = new Currency("USD");

        public Currency DefaultSourceCurrency {
            get => defaultSourceCurrency;
            set => defaultSourceCurrency = requireNotNull(value, "DefaultSourceCurrency");
        }

        public int CurrencyDigitsAfterPoint { get; set; } = 4;

    }
}