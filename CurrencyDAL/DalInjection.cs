using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyDAL
{

    public static class DalInjection
    {

        private static readonly CurrencyLayerCaller currencyLayerCaller = new CurrencyLayerCaller();

        private static readonly CurrencyContext currencyContext = new CurrencyContext();

        private static readonly CurrencyRefresh currencyRefresh = new CurrencyRefresh(currencyContext, currencyLayerCaller);

        private static readonly ICurrencyDataAccess currencyDataAccess = new CurrencyDataAccessImpl(currencyContext, currencyRefresh);


        public static ICurrencyDataAccess CurrencyDataAccessUnupdated => currencyDataAccess;

        public static async Task<ICurrencyDataAccess> GetCurrencyDataAccessAsync()
        {
            await currencyRefresh.UpdateHistoricalRatesAsync();
            await currencyRefresh.RefreshLiveRatesAsync();
            // TODO?: make the above a function in the currencyDataAccess
            return currencyDataAccess;
        } 

    }

}
