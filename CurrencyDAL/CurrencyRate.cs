using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyDAL
{
    public class CurrencyRate
    {
        public long Id { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
