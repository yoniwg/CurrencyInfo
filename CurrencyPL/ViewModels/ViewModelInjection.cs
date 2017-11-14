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

        private AppPreferences settings;

        public ViewModelInjection(ICurrencyBusinessLogic logic, AppPreferences settings)
        {
            this.logic = logic;
            this.settings = settings;
        }

        public AppPreferences ProvideAppPreference() => settings;

        public ConvertionVM ProvideConversionVM() => new ConvertionVM(logic, settings);

        public LiveRatesVM ProvideLiverateVM() => new LiveRatesVM(logic, settings);

        public HistoryVM ProvideHistoryVM() => new HistoryVM(logic, settings);

    }
}
