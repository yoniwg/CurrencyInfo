using CurrencyBE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyDAL
{
    public class CurrencyRateRecord
    {
        public long Id { get; set; }
        public Currency Source { get; set; }
        public decimal Rate { get; set; }
        public DateTime RateDate { get; set; }
    }
}
