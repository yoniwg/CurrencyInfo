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
    class CurrencyBusinessLogic : ICurrencyBusinessLogic
    {

        private readonly ICurrencyDataAccess data = new CurrencyDataAccess();

        private IReactiveProperty<CurrencyConverter> liveConverter;

        public IEnumerable<Currency> AvailableCurrencies => liveConverter.Value.AvailableCurrencies;

        public async Task init()
        {
            await data.initHistoricalDataAsync();
            await data.RefreshLiveRatesAsync();

            liveConverter = data.CurrencyRateRecords
                .SelectSwitchElements(_ => getLiveCurrencyConverterAsync())
                .ToReactiveProperty();
        }

        private Task<CurrencyConverter> getLiveCurrencyConverterAsync()
        {
            var lastDate = data.CurrencyRateRecords.Value
                .Select(rec => rec.RateDate)
                .Max();
            return getCurrencyConverterAsync(lastDate);
        }

        private async Task<CurrencyConverter> getCurrencyConverterAsync(DateTime date)
        {
            var dict = await data.CurrencyRateRecords.Value
                        .Where(rec => rec.RateDate.Date == date.Date)
                        .ToDictionaryAsync(rec => rec.Source, rec => rec.Rate);
            return new CurrencyConverter(dict);
        }

        private async Task<IList<decimal>> getHistoricalRatesToUSDAsync(Currency source, DateTime start, DateTime end)
        {
            return await data.CurrencyRateRecords.Value
                .Where(rec => rec.Source.Code == source.Code && start.Date <= rec.RateDate && rec.RateDate < end.Date)
                .GroupBy(rec => rec.RateDate)
                .OrderBy(dateRecGroup => dateRecGroup.Key)
                .Select(dateRecGroup => dateRecGroup.Select(rec => rec.Rate).Single())
                .ToListAsync();
        }

        private async Task<IDictionary<Currency, LiveRate>> liveRatesOfCurrencyAsync(Currency target)
        {
            var lastTwoDates = await data.CurrencyRateRecords.Value
                .Select(rec => rec.RateDate)
                .Distinct()
                .OrderByDescending(date => date)
                .Take(2)
                .Reverse()
                .ToArrayAsync();

            var converter1 = await getCurrencyConverterAsync(lastTwoDates[0]);
            var converter2 = await getCurrencyConverterAsync(lastTwoDates[1]);
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

        public async Task<HistoricalRate> GetHistoricalRate(Currency source, Currency target, DateTime start, DateTime end)
        {
            var sourceRates = await getHistoricalRatesToUSDAsync(source, start, end);
            var targetRates = await getHistoricalRatesToUSDAsync(source, start, end);
            var rates = targetRates.Zip(sourceRates, (x, y) => x / y);
            return new HistoricalRate(rates);
        }


        public IObservable<IDictionary<Currency, LiveRate>> LiveRatesOfCurrency(Currency target)
        {
            return data.CurrencyRateRecords.SelectSwitchElements(_ => liveRatesOfCurrencyAsync(target));
        }
    }
}
