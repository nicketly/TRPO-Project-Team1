using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TRPO_Project.WPFA.Model
{
    public class MoexService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<CurrencyRate> GetCurrencyRateAsync(string currency)
        {
            string url = $"https://iss.moex.com/iss/engines/currency/markets/selt/securities/{currency}.json";
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<MoexResponse>(content);

                var columns = data.MarketDataContainer.Columns;
                var lastIndex = columns.IndexOf("LAST");
                var rate = Convert.ToDecimal(data.MarketDataContainer.Data[0][lastIndex]);

                return new CurrencyRate
                {
                    Currency = currency,
                    Rate = rate
                };
            }
            else
            {
                throw new Exception("Unable to retrieve data from Moex.");
            }
        }
    }
}
