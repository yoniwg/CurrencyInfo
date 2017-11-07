
using CurrencyBE;
using CurrencyBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CurrencyPL.ViewModels
{
    public class HistoryVM : AbstractVM
    {

        private readonly ICurrencyBusinessLogic logic;

        public ICommand SelectRangeCommand { get; }

        public HistoryVM(ICurrencyBusinessLogic logic)
        {
            this.logic = logic;
            this.SelectRangeCommand = new AbstractCommand(param => SelectedRange = (HistoryRange)param);
        }

        public Currency TargetCurrency
        {
            get => GetValue(() => TargetCurrency);
            set => SetValue(() => TargetCurrency, value, RefreshGraph);
        }

        public Currency SourceCurency
        {
            get => GetValue(() => SourceCurency);
            set => SetValue(() => SourceCurency, value, RefreshGraph);
        }

        public HistoryRange SelectedRange
        {
            get => GetValue(() => SelectedRange);
            set => SetValue(() => SelectedRange, value, RefreshGraph);
        }

        private HistoricalRate History
        {
            get => GetValue(() => History);
            set => SetValue(() => History, value);
        }

        private void RefreshGraph()
        {
            DateTime start = GetStartDateForRange(SelectedRange);
            logic.GetHistoricalRate(SourceCurency, TargetCurrency, start, DateTime.Now);
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

    }

     public enum HistoryRange
    {
        WEEK, MONTH, YEAR
    }
}
