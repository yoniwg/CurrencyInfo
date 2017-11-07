using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyDAL
{

    public class CurrencyDataAccess : ICurrencyDataAccess
    {

        private readonly CurrencyContext currencyContext;

        private readonly CurrencyRefresh currencyReferesh;

        public event Action OnLiveRatesUpdated
        {
            add => currencyReferesh.OnLiveRateUpdated += value;
            remove => currencyReferesh.OnLiveRateUpdated -= value;
        }

        
        IQueryable<CurrencyRateRecord> ICurrencyDataAccess.CurrencyRateRecords => currencyContext.CurrencyRates;

        public CurrencyDataAccess()
        {
            currencyContext = new CurrencyContext();
            currencyContext.Database.EnsureCreated();


            currencyReferesh = new CurrencyRefresh(currencyContext, new CurrencyLayerCaller());

        }

        public async Task UpdateLiveRatesAsync() => await currencyReferesh.RefreshLiveRatesAsync();

        public async Task InitHistoricalDataAsync() => await currencyReferesh.UpdateHistoricalRatesAsync();
    }

}
