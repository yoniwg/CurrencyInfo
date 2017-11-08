using CurrencyBE;
using System;
using System.Collections.Generic;

namespace CurrencyBL
{
    public class HistoricalRate
    {

        /// <summary>
        /// A list of rates for each day starting with thw StartDay.
        /// </summary>
        public IList<decimal> Rates { get; }

        /// <summary>
        /// The first rated date
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// The (first) day after the last rated day.
        /// </summary>
        public DateTime EndDate { get => StartDate.AddDays(Rates.Count); }

        public HistoricalRate(Currency Source, Currency Target, DateTime startDate, IList<decimal> rates)
        {
            this.Rates = rates;
            this.StartDate = startDate.Date;
        }
    }
}