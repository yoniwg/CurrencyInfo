using CurrencyBE;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyBL
{
    internal class CurrencyConverter
    {

        private readonly Dictionary<Currency, decimal> rates;

        public CurrencyConverter(IDictionary<Currency, decimal> rates)
        {
            this.rates = new Dictionary<Currency, decimal>(rates);
        }

        public decimal RateOf(Currency source, Currency target)
        {
            return rates[source] / rates[target];
        }

        public IDictionary<Currency, decimal> RatesOf(Currency target) => AvailableCurrencies.ToDictionary(curr => curr, curr => RateOf(curr, target));

        public IEnumerable<Currency> AvailableCurrencies => rates.Keys;

    }
}