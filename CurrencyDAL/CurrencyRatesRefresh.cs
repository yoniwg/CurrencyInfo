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
    public class CurrencyRatesRefresh
    {
        private readonly CurrencyContext currencyContext = new CurrencyContext();
        private readonly CurrencyLayerCaller currencyLayerCaller = new CurrencyLayerCaller();
        private static readonly DateTime INITIAL_DATE = new DateTime(2015, 1, 1);
        private Timer refreshTodaysValueTimer;

        public async void UpdateDataAsync()
        {
            var lastRecord = getLastUpdatedRecord();
            var lastRecordDate = lastRecord.Date;
            lastRecordDate = lastRecordDate.AddDays(1);
            while (lastRecordDate.Date < DateTime.Today.Date)
            {
                var historicalRatesResponse = await currencyLayerCaller.GetHistoricalRatesResponseAsync(lastRecordDate);
                currencyContext.CurrencyRates.AddRange(historicalRatesResponse.ToCurrencyRatesOfDate(lastRecordDate));
                await currencyContext.SaveChangesAsync();
                Console.WriteLine("date: " + lastRecordDate + " updated");
                lastRecordDate = lastRecordDate.AddDays(1);
            }

            fireRefreshTodaysValueTimer();
        }

        private CurrencyRate getLastUpdatedRecord()
        {
            if (currencyContext.CurrencyRates.Any())
            {
                var maxDate = currencyContext.CurrencyRates.Max(rec => rec.Date);
                return currencyContext.CurrencyRates.First(rec => rec.Date == maxDate);
            }

            return new CurrencyRate { Date = INITIAL_DATE };

        }

        private void fireRefreshTodaysValueTimer()
        {
            refreshTodaysValueTimer = new Timer(refreshRates, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
        }

        private void refreshRates(object _)
        {
            try
            {
                var liveRatesResponse = currencyLayerCaller.GetLiveRatesResponseAsync().Result;
                var todaysRecords = currencyContext.CurrencyRates.Where(cr => cr.Date.Date == DateTime.Today.Date);
                var liveRates = liveRatesResponse.ToCurrencyRatesOfDate(DateTime.Now);
                if (todaysRecords.Any()) // if today's records already exist - update old
                {
                    foreach (var todaysRecord in todaysRecords)
                    {
                        var sameCurrency = liveRates.Single(tr => tr.CurrencyCode == todaysRecord.CurrencyCode);
                        todaysRecord.Date = sameCurrency.Date;
                        todaysRecord.Rate = sameCurrency.Rate;
                    }
                    currencyContext.CurrencyRates.UpdateRange(todaysRecords);
                }
                else                    // add them all
                {
                    currencyContext.CurrencyRates.AddRange(liveRates);
                }
                currencyContext.SaveChanges();
                Console.WriteLine("Today's rates refreshed at: " + DateTime.Now);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to refresh today's rates at: " + DateTime.Now, e);
            }
        }

    }
}
