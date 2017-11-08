using CurrencyBE;
using System.Collections.Generic;

namespace CurrencyBL
{
    public class LiveRate
    {
        public LiveRate() { }
        public LiveRate(Currency source, Currency target, decimal rate, decimal changeRatio)
        {
            this.Rate = rate;
            this.ChangeRatio = changeRatio;
            this.Target = target;
            this.Source = source;
        }

        public decimal Rate { get; set; }
        public decimal ChangeRatio { get; set; }
        public Currency Target { get; set; }
        public Currency Source { get; set; }
    }
}