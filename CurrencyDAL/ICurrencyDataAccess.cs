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
        
        event Action OnLiveRatesUpdated;

        IQueryable<CurrencyRateRecord> CurrencyRateRecords { get; }

        Task InitHistoricalDataAsync();

        Task UpdateLiveRatesAsync();

    }
}
