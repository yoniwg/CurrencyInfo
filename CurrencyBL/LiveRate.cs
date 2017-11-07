using CurrencyBE;
using System.Collections.Generic;

namespace CurrencyBL
{
    public class LiveRate
    {

        public LiveRate(Currency source, Currency target, decimal rate, decimal changeRatio)
        {
            this.Rate = rate;
            this.ChangeRatio = changeRatio;
            this.Target = target;
            this.Source = source;
        }

        public decimal Rate { get; }
        public decimal ChangeRatio { get; }
        public Currency Target { get; }
        public Currency Source { get; }
    }
}