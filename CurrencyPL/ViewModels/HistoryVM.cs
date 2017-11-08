
using CurrencyBE;
using CurrencyBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace CurrencyPL.ViewModels
{
    public class HistoryVM : AbstractVM
    {

        private readonly ICurrencyBusinessLogic logic;
        
        public HistoryVM(ICurrencyBusinessLogic logic, AppPreferences prefs)
        {
            this.logic = logic;
            this.FlipCurrenciesCommand = new AbstractCommand(_ =>
            {
                var oldSource = SourceCurrency;
                var oldTarget = TargetCurrency;
                SourceCurrency = oldTarget;
                TargetCurrency = oldSource;
            });
            this.AvailableCurrencies = logic.AvailableCurrencies.ToArray();

            SourceCurrency = prefs.DefaultSourceCurrency;
            TargetCurrency = prefs.MainTargetCurrency;

            WeekChecked = true;
        }

        public Currency TargetCurrency
        {
            get => GetValue(() => TargetCurrency);
            set => SetValue(() => TargetCurrency, value, RefreshGraph);
        }

        public Currency SourceCurrency
        {
            get => GetValue(() => SourceCurrency);
            set => SetValue(() => SourceCurrency, value, RefreshGraph);
        }

        public bool WeekChecked
        {
            get => GetValue(() => WeekChecked);
            set => SetValue(() => WeekChecked, value, () => SelectedRange = HistoryRange.WEEK);
        }

        public bool MonthChecked
        {
            get => GetValue(() => MonthChecked);
            set => SetValue(() => MonthChecked, value, () => SelectedRange = HistoryRange.MONTH);
        }

        public bool YearChecked
        {
            get => GetValue(() => YearChecked);
            set => SetValue(() => YearChecked, value, () => SelectedRange = HistoryRange.YEAR);
        }


        public HistoryRange SelectedRange
        {
            get => GetValue(() => SelectedRange);
            set => SetValue(() => SelectedRange, value, RefreshGraph);
        }
        
        public IList<KeyValuePair<object,double>> GraphPairs
        {
            get => GetValue(() => GraphPairs);
            set => SetValue(() => GraphPairs, value);
        }

        public IList<Currency> AvailableCurrencies { get; }

        private void RefreshGraph()
        {
            if (SelectedRange == null || SourceCurrency == null || TargetCurrency == null) return;
            DateTime start = GetStartDateForRange(SelectedRange);
            var history = logic.GetHistoricalRate(SourceCurrency, TargetCurrency, start, DateTime.Now);
            GraphPairs = historyToPairs(history);
        }

        private IList<KeyValuePair<object, double>> historyToPairs(HistoricalRate history)
        {
            int count = history.Rates.Count;
            int lastIndex = count - 1;
            var maxPoints = 31;
            var step = Math.Max(count / maxPoints, 1);
            var start = history.StartDate;

            object KeyForRate(int i)
            {
                var date = start.AddDays(i);
                //return date.ToShortDateString();
                return date;
            };

            var rates = history.Rates;
            IList<KeyValuePair<object, double>> pairs;

            if (rates.Count <= maxPoints)
            {
                pairs = rates
                    .Select((rate, i) => new KeyValuePair<object, double>(KeyForRate(i), (double)rate))
                    .ToArray();
            } else
            {
                pairs = rates
                    .Select((rate, i) => new KeyValuePair<object, double>(KeyForRate(i), (double)rate))
                    .Where((_,i)=> (lastIndex - i) % step == 0 || i == lastIndex)
                    .ToArray();
            }

            // pairs = pairs.Select((pair, i) => new KeyValuePair<object, double>(i % (pairs.Count / 5) == 0 ? pair.Key : ".", pair.Value)).ToArray();
            
            return pairs;
        }

        private DateTime GetStartDateForRange(HistoryRange range)
        {
            switch (range)
            {
                case HistoryRange.WEEK:
                    return DateTime.Now.AddDays(-7);
                case HistoryRange.MONTH:
                    return DateTime.Now.AddMonths(-1);
                case HistoryRange.YEAR:
                    return DateTime.Now.AddYears(-1);
                default:
                    throw new NotImplementedException("unexpected enum value.");
            }
        }


        public ICommand FlipCurrenciesCommand { get; }


    }

  

    public enum HistoryRange
    {
        WEEK, MONTH, YEAR
    }
}
