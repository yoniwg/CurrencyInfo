using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.DateTimeFormatting;
using Newtonsoft.Json;

namespace CurrencyDAL
{
    public class CurrencyLayerCaller
    {
        private const string CURRENCY_LAYER_URL_FORMAT = "http://www.apilayer.net/api/{0}?access_key=ebf81dcac83466510f577f8411017684&{1}";
        private const string LIVE_ADDRESS = "live";
        private const string HISTORICAL_ADDRESS = "historical";

        public async Task<CurrencyLayerResponse> GetLiveRatesResponseAsync()
        {
            return await HttpGetCurrencyLayerAsync(LIVE_ADDRESS);
        }

        public async Task<CurrencyLayerResponse> GetHistoricalRatesResponseAsync(DateTime ofDay)
        {

            var formatedDate = string.Format("{0:yyyy-MM-dd}", ofDay);
            return await HttpGetCurrencyLayerAsync(HISTORICAL_ADDRESS, "date=" + formatedDate);

        }

        private static async Task<CurrencyLayerResponse> HttpGetCurrencyLayerAsync(string address, params string[] prms)
        {
            var joinedParams = (prms.Length > 0) ? prms.Aggregate((a, b) => a + "&" + b) : "";
            var requestUriString = String.Format(CURRENCY_LAYER_URL_FORMAT,address, joinedParams);
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);

                var httpResponse = await httpWebRequest.GetResponseAsync();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = await streamReader.ReadToEndAsync();
                    var response = JsonConvert.DeserializeObject<CurrencyLayerResponse>(responseText);
                    if (!response.success)
                    {
                        throw response.error;
                    }
                    return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Currency Layer GET: {0},{1}", requestUriString, e);
                throw;
            }
        }
    }

    public class CurrencyLayerResponse
    {
        public CurrencyLayerResponseException error { get; set; }
        public bool success { get; set; }
        public IDictionary<string, decimal> quotes { get; set; }

        public ISet<CurrencyRate> ToCurrencyRatesOfDate(DateTime date)
        {
            return quotes.Select(quote => new CurrencyRate
            {
                Date = date,
                CurrencyCode = quote.Key.Substring(3), // Remove USD prefix
                Rate = quote.Value
            }).ToImmutableHashSet();
        }
    }

    public class CurrencyLayerResponseException : Exception
    {
        public int code { get; set; }
        public string info { get; set; }
    }
}
