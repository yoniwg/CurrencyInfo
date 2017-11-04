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

        public IReactiveProperty<IQueryable<CurrencyRateRecord>> CurrencyRateRecords { get; private set; }

        public CurrencyDataAccess()
        {
            currencyContext = new CurrencyContext();
            currencyReferesh = new CurrencyRefresh(currencyContext, new CurrencyLayerCaller());

            this.CurrencyRateRecords = Observable.FromEvent(action => currencyReferesh.OnLiveRateUpdated += action,
                                                    action => currencyReferesh.OnLiveRateUpdated -= action)
                                                    .StartWith(Unit.Default)
                                                    .Select(_=> currencyContext.CurrencyRates.AsQueryable())
                                                    .ToReactiveProperty();
        }

        public async Task RefreshLiveRatesAsync() => await currencyReferesh.RefreshLiveRatesAsync();

        public async Task initHistoricalDataAsync() => await currencyReferesh.UpdateHistoricalRatesAsync();
    }

}
