namespace CurrencyBL
{
    public class Currency
    {

        public string Code { get; }

        internal Currency(string currencyCode)
        {
            this.Code = currencyCode;
        }

    }
}