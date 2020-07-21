using KursyWalut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KursyWalut.Repositories.Interfaces
{
    public interface IRatesRepository
    {
        public Task<Rate> GetCurrencyActualRateAsync(string currencyCode);
        public Task<List<Rate>> GetCurencyRatesInDateRangeAsync(string currencyCode, DateTime startDate, DateTime endDate);
    }
}
