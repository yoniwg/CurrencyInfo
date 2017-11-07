using CurrencyBE;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyDAL
{
    public class CurrencyRateRecord
    {
        private string _source;

        public long Id { get; set; }

        [NotMapped]
        public Currency Source {
            get => new Currency(_source);
            set => _source = value.Code;
        }

        public decimal Rate { get; set; }
        public DateTime RateDate { get; set; }

        

    }
}
