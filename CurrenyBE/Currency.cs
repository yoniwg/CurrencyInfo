namespace CurrencyBE
{
    public class Currency
    {

        public string Code { get; }

        public Currency(string currencyCode)
        {
            this.Code = currencyCode;
        }

        //TODO Equals HashCode

    }
}