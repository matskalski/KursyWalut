using KursyWalut.Models;
using KursyWalut.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KursyWalut.Repositories
{
    public class RatesRepository : IRatesRepository
    {
        public async Task<Rate> GetCurrencyActualRateAsync(string currencyCode)
        {
            string url = string.Format(CultureInfo.InvariantCulture,
                                       "http://api.nbp.pl/api/exchangerates/rates/a/{0}/?format=json",
                                        Uri.EscapeDataString(currencyCode));

            Root rootData;

            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(url);
                rootData = JsonConvert.DeserializeObject<Root>(json);
            }


            return rootData.rates.FirstOrDefault();
        }


        public async Task<List<Rate>> GetCurencyRatesInDateRangeAsync(string currencyCode, DateTime startDate, DateTime endDate)
        {
            


            string url = string.Format(CultureInfo.InvariantCulture,
                                       "http://api.nbp.pl/api/exchangerates/rates/a/{0}/{1}/{2}/?format=json",
                                        Uri.EscapeDataString(currencyCode),
                                        Uri.EscapeDataString(startDate.ToString("yyyy-MM-dd")),
                                        Uri.EscapeDataString(endDate.ToString("yyyy-MM-dd")));

            Root rootData;

            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(url);
                rootData = JsonConvert.DeserializeObject<Root>(json);
            }

            return rootData.rates;
        }
    }
}
