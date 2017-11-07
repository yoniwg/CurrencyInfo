
using CurrencyBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CurrencyPL.ViewModels
{
    class HistoryVM : AbstractVM
    {

        private readonly ICurrencyBusinessLogic logic;

        public ICommand SelectRangeCommand { get; }

        public HistoryVM(ICurrencyBusinessLogic logic)
        {
            this.logic = logic;
            this.SelectRangeCommand = new AbstractCommand(OnSelectRange);
        }


        private void OnSelectRange(object input)
        {
            var historyRange = (HistoryRange)input;
            switch (historyRange)
            {
                default:
                    throw new NotImplementedException("unexpected enum value.");
                    break;
            }
        }

    }

        enum HistoryRange
    {
        WEEK, MONTH, YEAR
    }
}
