using System;

namespace CurrencyBE
{
    public class Currency
    {

        public string Code { get; }

        public Currency(string currencyCode)
        {
            if (currencyCode == null) throw new ArgumentNullException("currencyCode is null");
            this.Code = currencyCode;
        }

        public override bool Equals(object obj) => obj is Currency && ((Currency)obj).Code == Code;


        public override int GetHashCode() => Code.GetHashCode();

        public override string ToString()
        {
            return Code;
        }
    }
}