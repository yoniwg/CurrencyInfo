using CurrencyBE;
using CurrencyBL;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CurrencyPL.ViewModels
{
    public class ConvertionVM : AbstractVM
    {

        public Currency SourceCurrency {
            get => GetValue(() => SourceCurrency);
            set => SetValue(() => SourceCurrency, value, RefreshConvertion);
        }

        public Currency TargetCurrency
        {
            get => GetValue(() => TargetCurrency);
            set => SetValue(() => TargetCurrency, value, RefreshConvertion);
        }

        public decimal SourceAmount
        {
            get => GetValue(() => SourceAmount);
            set => SetValue(() => SourceAmount, value, RefreshConvertion);
        }

        public decimal TargetAmount
        {
            get => GetValue(() => TargetAmount);
            set => SetValue(() => TargetAmount, value);
        }

        public IList<Currency> AvailableCurrencies { get; }
 
        private readonly ICurrencyBusinessLogic logic;

        private void RefreshConvertion()
        {
            if (SourceCurrency == null || TargetCurrency == null) return;
            TargetAmount = logic.ConvertCurrencies(SourceCurrency, TargetCurrency) * SourceAmount;
        }


        public ConvertionVM(ICurrencyBusinessLogic logic)
        {
            this.logic = logic;

            AvailableCurrencies = logic.AvailableCurrencies.ToArray();// (new string[] { "USD", "ILS" }.Select(s => new Currency(s))).ToArray();

            FlipCurrenciesCommand = new AbstractCommand(e => {
                var oldSource = SourceCurrency;
                var oldTarget = TargetCurrency;
                SourceCurrency = oldTarget;
                TargetCurrency = oldSource;
                SourceAmount = TargetAmount;
                // The TargetAmount will be automatcally updated.
            });


            SourceCurrency = AvailableCurrencies[0];
            TargetCurrency = AvailableCurrencies[1];
        }

        ICommand FlipCurrenciesCommand {
            get => GetValue(() => FlipCurrenciesCommand);
            set => SetValue(() => FlipCurrenciesCommand, value);
        } 



    }
}
