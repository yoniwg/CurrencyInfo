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
    class CurrencyConvertionVM : AbstractVM
    {

        Currency SourceCurrency {
            get => GetValue(() => SourceCurrency);
            set => SetValue(() => SourceCurrency, value, RefreshConvertion);
        }

        Currency TargetCurrency
        {
            get => GetValue(() => TargetCurrency);
            set => SetValue(() => TargetCurrency, value, RefreshConvertion);
        }

        decimal SourceAmount
        {
            get => GetValue(() => SourceAmount);
            set => SetValue(() => SourceAmount, value, RefreshConvertion);
        }

        decimal TargetAmount
        {
            get => GetValue(() => TargetAmount);
            set => SetValue(() => TargetAmount, value);
        }


        private readonly ICurrencyBusinessLogic logic;

        private void RefreshConvertion()
        {
            TargetAmount = logic.ConvertCurrencies(SourceCurrency, TargetCurrency) * SourceAmount;
        }


        CurrencyConvertionVM(ICurrencyBusinessLogic logic)
        {
            this.logic = logic;

            FlipCurrencies = new AbstractCommand(e => {
                var oldSource = SourceCurrency;
                var oldTarget = TargetCurrency;
                SourceCurrency = oldTarget;
                TargetCurrency = oldSource;
                SourceAmount = TargetAmount;
                // The TargetAmount will be automatcally updated.
            });
        }

        ICommand FlipCurrencies { get; } 



    }
}
