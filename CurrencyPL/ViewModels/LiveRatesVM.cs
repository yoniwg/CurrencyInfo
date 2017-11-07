using CurrencyBE;
using CurrencyBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyPL.ViewModels
{
    class LiveRatesVM : AbstractVM
    {
        private readonly ICurrencyBusinessLogic logic;

        public LiveRatesVM(ICurrencyBusinessLogic logic)
        {
            this.logic = logic;
            TargetCurrency = null; // TODO
            var sourceCurrencies = logic.AvailableCurrencies; // TODO
            logic.LiveRatesOfCurrency(TargetCurrency).Subscribe(liveRatesMap =>
            {
                LiveRates = liveRatesMap
                    .Where(pair => sourceCurrencies.Contains(pair.Key))
                    .Select(pair => pair.Value)
                    .ToArray();
            });
        }

        public Currency TargetCurrency { get; }

        public IList<LiveRate> LiveRates {
            get => GetValue(() => LiveRates);
            private set => SetValue(() => LiveRates, value);
        }

    }

}