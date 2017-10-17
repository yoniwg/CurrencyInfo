using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyDAL
{
    public interface ICurrencyDataAccess 
    {
        
        IQueryable<CurrencyRateRecord> CurrenciesRates { get; }

        event Action OnLiveRatesRefresh;

    }
}
