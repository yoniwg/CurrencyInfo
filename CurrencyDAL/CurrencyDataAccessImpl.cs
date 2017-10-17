using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyDAL
{

    class CurrencyDataAccessImpl : ICurrencyDataAccess
    {

        private readonly CurrencyContext context;

        private readonly CurrencyRefresh refreshService;

        public CurrencyDataAccessImpl(CurrencyContext currencyContext, CurrencyRefresh currencyReferesh)
        {
            this.context = currencyContext;
            this.refreshService = currencyReferesh;
        }

        public IQueryable<CurrencyRateRecord> CurrenciesRates => throw new NotImplementedException();

        public event Action OnLiveRatesRefresh
        {
            add => refreshService.OnLiveRateUpdated += value;
            remove => refreshService.OnLiveRateUpdated -= value;
        }

    }

}
