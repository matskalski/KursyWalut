using KursyWalut.Enums;
using KursyWalut.Models;
using KursyWalut.Repositories.Interfaces;
using KursyWalut.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KursyWalut.ViewModels;
using OfficeOpenXml;
using System.Data;
using FastMember;

namespace KursyWalut.Services
{
    public class RatesService : IRatesService
    {
        private readonly IRatesRepository _ratesRepository;

        public RatesService(IRatesRepository ratesRepository)
        {
            _ratesRepository = ratesRepository;
        }

        public async Task<List<Rate>> GetCurrenciesActualRateAsync(List<string> codes)
        {
            List<Rate> rates = new List<Rate>();

            foreach (var code in codes)
            {
                try
                {
                    var result = await _ratesRepository.GetCurrencyActualRateAsync(code);
                    result.code = code.ToUpper();
                    rates.Add(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return rates;
        }

        public async Task<List<Rate>> GetCurencyActualMonthRatesAsync(int codeId)
        {
            CurrencyCodes code = (CurrencyCodes)codeId;
            var firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var result = await _ratesRepository.GetCurencyRatesInDateRangeAsync(code.ToString(), firstDayOfMonth, DateTime.Today);
            return result;

        }

        public async Task<List<AverageTable>> GetCurencyActualYearAverageRatesAsync(List<string> codes)
        {
            var list = new List<AverageRate>();
            var firstDayOfYear = new DateTime(DateTime.Today.Year, 1, 1);
            
            foreach (var code in codes)
            {
                var result = await _ratesRepository.GetCurencyRatesInDateRangeAsync(code.ToString(), firstDayOfYear, DateTime.Today);
                foreach (var r in result)
                {
                    r.code = code;
                }

                var itemList = result.GroupBy(gb => new { Code = gb.code, Month = DateTime.Parse(gb.effectiveDate).Month })
                                 .Select(s => new
                                 {
                                     AvgRate = s.Average(a => a.mid),
                                     Month = s.Key.Month,
                                     Code = s.Key.Code.ToUpper()
                                 });

                foreach (var item in itemList)
                {
                    AverageRate averageRate = new AverageRate
                    {
                        AvgRate = item.AvgRate,
                        CurrencyCode = item.Code,
                        Month = item.Month
                    };

                    list.Add(averageRate);
                }
            }

            var final = from lusd in list
                        join leur in list on lusd.Month equals leur.Month
                        where lusd.CurrencyCode.Equals("USD") &&
                              leur.CurrencyCode.Equals("EUR")
                        select new
                        {
                            Month = lusd.Month,
                            UsdAvgRate = lusd.AvgRate,
                            EurAvgRate = leur.AvgRate
                        };

            var finalList = new List<AverageTable>();

            foreach (var item in final)
            {
                var at = new AverageTable
                {
                    Month = GetMonthName(item.Month),
                    UsdRate = item.UsdAvgRate,
                    EurRate = item.EurAvgRate
                };

                finalList.Add(at);
            }
            
            return finalList;
        }

        public async Task<byte[]> PrepareFileContent(List<Rate> rates, int codeId)
        {
            CurrencyCodes code = (CurrencyCodes)codeId;
            foreach (var rate in rates)
            {

                rate.code = code.ToString();
            }

            RatesHistoryVM model = new RatesHistoryVM
            {
                rates = rates
            };

            #region licences
            // If you are a commercial business and have
            // purchased commercial licenses use the static property
            // LicenseContext of the ExcelPackage class :
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            #endregion

            DataTable dataTable = new DataTable();
            using (var reader = ObjectReader.Create(model.rates))
            {
                dataTable.Load(reader);
            }

            Task<byte[]> fileContent;
            using (ExcelPackage package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("RatesHistory");
                workSheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                fileContent = package.GetAsByteArrayAsync();
            }

            return await fileContent;
        }

        private string GetMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "styczeń";                    
                case 2:
                    return "luty";
                case 3:
                    return "marzec";
                case 4:
                    return "kwiecień";
                case 5:
                    return "maj";
                case 6:
                    return "czerwiec";
                case 7:
                    return "lipiec";
                case 8:
                    return "sierpień";
                case 9:
                    return "wrzesień";
                case 10:
                    return "październik";
                case 11:
                    return "listopad";
                case 12:
                    return "grudzień";
                default:
                    return "-";
                    
            }

        }
    }
}
