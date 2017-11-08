using CurrencyBE;
using CurrencyBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyPL.ViewModels
{
    public class LiveRatesVM : AbstractVM
    {
        private readonly ICurrencyBusinessLogic logic;
        public IList<Currency> AvailableCurrencies { get; }
        public IList<Currency> SourceCurrencies { get; }

        public LiveRatesVM(ICurrencyBusinessLogic logic, AppPreferences prefs)
        {
            this.logic = logic;
            AvailableCurrencies = logic.AvailableCurrencies.ToArray();
            SourceCurrencies = AvailableCurrencies;
            TargetCurrency = prefs.MainTargetCurrency;

        }

        public Currency TargetCurrency
        {
            get => GetValue(() => TargetCurrency);
            set => SetValue(() => TargetCurrency, value, RefreshRates);
        }

        private void RefreshRates()
        {
            var foo = logic.LiveRatesOfCurrencyDic(TargetCurrency);
             var foo1 = foo
                .Where(pair => SourceCurrencies.Contains(pair.Key));
            var foo2 = foo1
                .Select(pair => pair.Value);
            LiveRates = foo2.ToList();

            //            logic.LiveRatesOfCurrency(TargetCurrency).Subscribe(liveRatesMap =>
            //            {
            //                LiveRates = liveRatesMap
            //                    .Where(pair => SourceCurrencies.Contains(pair.Key))
            //                    .Select(pair => pair.Value)
            //                    .ToArray();
            //            });
        }

        public IList<LiveRate> LiveRates {
            get => GetValue(() => LiveRates);
            private set => SetValue(() => LiveRates, value);
        }

    }

}