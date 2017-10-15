using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyBE
{
    public class CurrenciesRate
    {
        // TODO

        public CurrenciesRate(IDictionary<Currency, decimal> rates)
        {
            this.rates = rates;
        }

        private IDictionary<Currency, decimal> rates { get; }

        public decimal? Convert(Currency source, Currency target)
        {
            return null;
        }

        public IEnumerable<string> GetRateFrom(Currency source)
        {
            return null;
        }

    }
}
