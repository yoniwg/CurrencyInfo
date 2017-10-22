using CurrencyBE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyBL
{

    public interface ICurrencyBusinessLogic
    {

        IEnumerable<Currency> AvailableCurrencies { get; }

        decimal ConvertCurrencies(Currency source, Currency target);

        IObservable<IDictionary<Currency, LiveRate>> LiveRatesOfCurrency(Currency source);


        /// <summary>Get a list of historical rates of certain currencies.</summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="start">The start of the date range.</param>
        /// <param name="end"> The end of the day range, exclusive.</param>
        /// <returns>A list of rates for each day in the range.</returns>
        Task<HistoricalRate> GetHistoricalRate(Currency source, Currency target, DateTime start, DateTime end);

    }


}
