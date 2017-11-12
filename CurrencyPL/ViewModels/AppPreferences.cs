using CurrencyBE;
using CurrencyBL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CurrencyPL.ViewModels
{
    public class AppPreferences : INotifyPropertyChanged
    {

        public AppPreferences(ICurrencyBusinessLogic logic)
        {
            this.logic = logic;
        }

        public IList<Currency> AvailableCurrencies => logic.AvailableCurrencies.ToArray();

        private Currency mainTargetCurrency = new Currency("ILS");

        public Currency MainTargetCurrency {
            get => mainTargetCurrency;
            set
            {
                mainTargetCurrency = requireNotNull(value, "MainTargetCurrency");
                NotifyPropertyChanged("MainTargetCurrency");
            }
        }

        private T requireNotNull<T>(T value, string propertyName)
        {
            if (value == null) throw new ArgumentNullException("trying to assign null into '" + propertyName + "' property.");
            return value;
        }

        private Currency defaultSourceCurrency = new Currency("USD");
        private ICurrencyBusinessLogic logic;

        public event PropertyChangedEventHandler PropertyChanged;

        public Currency DefaultSourceCurrency {
            get => defaultSourceCurrency;
            set
            {
                defaultSourceCurrency = requireNotNull(value, "DefaultSourceCurrency");
                NotifyPropertyChanged("DefaultSourceCurrency");
            }
        }

        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public int CurrencyDigitsAfterPoint { get; set; } = 4;

    }
}