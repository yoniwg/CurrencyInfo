using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyBL;

namespace CurrencyPL.ViewModels
{
    public class ViewModelInjection
    {

        private ICurrencyBusinessLogic logic;

        public ViewModelInjection(ICurrencyBusinessLogic logic)
        {
            this.logic = logic;
        }

        public ConvertionVM ProvideConversionVM() => new ConvertionVM(logic);

        public LiveRatesVM ProvideLiverateVM() => new LiveRatesVM(logic);

        public HistoryVM ProvideHistoryVM() => new HistoryVM(logic);

    }
}
