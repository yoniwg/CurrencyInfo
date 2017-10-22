using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CurrencyDAL
{
     class CurrencyRefresh
    {

        private readonly CurrencyContext currencyContext;

        private readonly CurrencyLayerCaller currencyLayerCaller;

        private static readonly DateTime INITIAL_DATE = new DateTime(2015, 1, 1);

        private Timer refreshTodaysValueTimer;


        public CurrencyRefresh(CurrencyContext currencyContext, CurrencyLayerCaller currencyLayerCaller)
        {
            this.currencyContext = currencyContext;
            this.currencyLayerCaller = currencyLayerCaller;

            OnLiveRateUpdated += FireRefreshTodaysValueTimer;
        }

        public event Action OnLiveRateUpdated;


        public async Task UpdateHistoricalRatesAsync()
        {
            var lastRecord = getLastUpdatedRecord();
            var lastRecordDate = lastRecord.RateDate;
            lastRecordDate = lastRecordDate.AddDays(1);
            while (lastRecordDate.Date < DateTime.Today.Date)
            {
                var historicalRatesResponse = await currencyLayerCaller.GetHistoricalRatesResponseAsync(lastRecordDate);
                currencyContext.CurrencyRates.AddRange(historicalRatesResponse.ToCurrencyRatesOfDate(lastRecordDate));
                await currencyContext.SaveChangesAsync();
                Console.WriteLine("date: " + lastRecordDate + " updated");
                lastRecordDate = lastRecordDate.AddDays(1);
            }

        }


        private CurrencyRateRecord getLastUpdatedRecord()
        {
            if (currencyContext.CurrencyRates.Any())
            {
                var maxDate = currencyContext.CurrencyRates.Max(rec => rec.RateDate);
                return currencyContext.CurrencyRates.First(rec => rec.RateDate == maxDate);
            }

            return new CurrencyRateRecord { RateDate = INITIAL_DATE };

        }

        public void FireRefreshTodaysValueTimer()
        {
            refreshTodaysValueTimer = new Timer(async _ => await RefreshLiveRatesAsync(), null, TimeSpan.Zero, TimeSpan.FromMinutes(30));
        }

        public async Task RefreshLiveRatesAsync()
        {
            try
            {
                var liveRatesResponse = await currencyLayerCaller.GetLiveRatesResponseAsync();
                var todaysRecords = currencyContext.CurrencyRates.Where(cr => cr.RateDate.Date == DateTime.Today.Date);
                var liveRates = liveRatesResponse.ToCurrencyRatesOfDate(DateTime.Now);
                if (await todaysRecords.AnyAsync()) // if today's records already exist - update old
                {
                    foreach (var todaysRecord in todaysRecords)
                    {
                        var sameCurrency = liveRates.Single(tr => tr.Source == todaysRecord.Source);
                        todaysRecord.RateDate = sameCurrency.RateDate;
                        todaysRecord.Rate = sameCurrency.Rate;
                    }
                    currencyContext.CurrencyRates.UpdateRange(todaysRecords);
                }
                else                    // add them all
                {
                    currencyContext.CurrencyRates.AddRange(liveRates);
                }
                await currencyContext.SaveChangesAsync();
                Console.WriteLine("Today's rates refreshed at: " + DateTime.Now);
                OnLiveRateUpdated();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to refresh today's rates at: " + DateTime.Now, e);
            }
        }

    }
}
