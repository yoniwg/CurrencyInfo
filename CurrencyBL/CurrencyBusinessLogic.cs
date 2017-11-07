using CurrencyBE;
using CurrencyDAL;
using Microsoft.EntityFrameworkCore;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyBL
{
    public class CurrencyBusinessLogic : ICurrencyBusinessLogic
    {

        private readonly ICurrencyDataAccess data = new CurrencyDataAccess();

        private ReactiveProperty<CurrencyConverter> liveConverter = new ReactiveProperty<CurrencyConverter>();

        public IEnumerable<Currency> AvailableCurrencies => liveConverter.Value.AvailableCurrencies;

        private Task initTask;

        public void Init()
        {
            initActual();
            /*
            // avoid multiple initiializations.
            if (initTask == null || initTask.IsCanceled || initTask.IsFaulted)
            {
                initTask = initActual();
            }
            await initTask;
            */
        }

        public void initActual()
        {
                data.InitHistoricalDataAsync();
                data.UpdateLiveRatesAsync();
                RefreshLiveConverterAsync();
                data.OnLiveRatesUpdated += () => RefreshLiveConverterAsync();
            
        }
        
        private void RefreshLiveConverterAsync()
        {
            liveConverter.Value = getLiveCurrencyConverter();
        }

        private CurrencyConverter getLiveCurrencyConverter()
        {
            var lastDate = data.CurrencyRateRecords
                .Select(rec => rec.RateDate)
                .Max();
            return getCurrencyConverter(lastDate);
        }

        private CurrencyConverter getCurrencyConverter(DateTime date)
        {
                var dict = data.CurrencyRateRecords
                        .Where(rec => rec.RateDate.Date == date.Date)
                        .ToDictionary(rec => rec.Source, rec => rec.Rate);
            return new CurrencyConverter(dict);
        }

        private IList<decimal> getHistoricalRatesToUSD(Currency source, DateTime start, DateTime end)
        {
            return data.CurrencyRateRecords
                .Where(rec => rec.Source.Code == source.Code && start.Date <= rec.RateDate && rec.RateDate < end.Date)
                .GroupBy(rec => rec.RateDate)
                .OrderBy(dateRecGroup => dateRecGroup.Key)
                .Select(dateRecGroup => dateRecGroup.Select(rec => rec.Rate).Single())
                .ToList();
        }

        private IDictionary<Currency, LiveRate> liveRatesOfCurrency(Currency target)
        {
            var lastTwoDates = data.CurrencyRateRecords
                .Select(rec => rec.RateDate)
                .Distinct()
                .OrderByDescending(date => date)
                .Take(2)
                .Reverse()
                .ToArray();

            var converter1 = getCurrencyConverter(lastTwoDates[0]);
            var converter2 = getCurrencyConverter(lastTwoDates[1]);
            var availableCurrencies = converter1.AvailableCurrencies.Intersect(converter2.AvailableCurrencies);
            Func<Currency, LiveRate> currencyToLiveRate = sourceCurrency =>
            {
                var rate1 = converter1.RateOf(sourceCurrency, target);
                var rate2 = converter2.RateOf(sourceCurrency, target);
                return new LiveRate(rate2, rate2 / rate1);
            };

            return availableCurrencies.ToDictionary(currency => currency, currencyToLiveRate);
        }


        public decimal ConvertCurrencies(Currency source, Currency target)
        {
            return liveConverter.Value.RateOf(source, target);
        }

        public HistoricalRate GetHistoricalRate(Currency source, Currency target, DateTime start, DateTime end)
        {
            var sourceRates = getHistoricalRatesToUSD(source, start, end);
            var targetRates = getHistoricalRatesToUSD(source, start, end);
            var rates = targetRates.Zip(sourceRates, (x, y) => x / y);
            return new HistoricalRate(rates);
        }


        public IObservable<IDictionary<Currency, LiveRate>> LiveRatesOfCurrency(Currency target)
        {
            return Observable
                .FromEvent(h => data.OnLiveRatesUpdated += h, h => data.OnLiveRatesUpdated -= h)
                .Select(_ => liveRatesOfCurrency(target));
        }
    }
}
