using System.Collections.Generic;

namespace CurrencyBL
{
    public class LiveRate
    {

        public LiveRate(decimal rate, decimal changeRatio)
        {
            this.Rate = rate;
            this.ChangeRatio = changeRatio;
        }

        public decimal Rate { get; }
        public decimal ChangeRatio { get; }

    }
}