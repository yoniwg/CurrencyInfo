using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyDAL
{
    public interface ICurrencyDataAccess 
    {
        
        IReactiveProperty<IQueryable<CurrencyRateRecord>> CurrencyRateRecords { get; }

        Task initHistoricalDataAsync();

        Task RefreshLiveRatesAsync();

    }
}
