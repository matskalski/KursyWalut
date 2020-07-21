using KursyWalut.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KursyWalut.Services.Interfaces
{
    public interface IRatesService
    {
        public Task<List<Rate>> GetCurrenciesActualRateAsync(List<string> codes);
        public Task<List<Rate>> GetCurencyActualMonthRatesAsync(int codeId);
        public Task<List<AverageTable>> GetCurencyActualYearAverageRatesAsync(List<string> codes);
        public Task<byte[]> PrepareFileContent(List<Rate> rates, int codeId);
    }
}
