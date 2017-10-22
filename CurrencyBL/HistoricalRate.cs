using System.Collections.Generic;

namespace CurrencyBL
{
    public class HistoricalRate
    {

        public IEnumerable<decimal> Rates { get; }

        public HistoricalRate(IEnumerable<decimal> rates)
        {
            this.Rates = rates;
        }
    }
}