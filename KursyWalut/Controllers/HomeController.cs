using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KursyWalut.Models;
using KursyWalut.Services.Interfaces;
using KursyWalut.ViewModels.Home;
using KursyWalut.ViewModels;
using OfficeOpenXml;
using System.Data;
using FastMember;
using KursyWalut.Enums;

namespace KursyWalut.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRatesService _ratesService;

        public HomeController(ILogger<HomeController> logger, IRatesService rateService)
        {
            _logger = logger;
            _ratesService = rateService;
        }

        public async Task<IActionResult> Index()
        {
            List<string> codes = new List<string>();
            codes.Add("usd");
            codes.Add("eur");

            var rates = await _ratesService.GetCurrenciesActualRateAsync(codes);
            var avgRates = await _ratesService.GetCurencyActualYearAverageRatesAsync(codes);

            IndexVM model = new IndexVM
            {
                Rates = rates,
                AvarageRates = avgRates

            };

            var r = await _ratesService.GetCurencyActualYearAverageRatesAsync(codes);

            return await Task.Run(() => View(model));

        }

        [HttpGet]
        public async Task<IActionResult> GetRateHistory(int id)
        {
            var rates = await _ratesService.GetCurencyActualMonthRatesAsync(id);

            RatesHistoryVM model = new RatesHistoryVM
            {
                currencyId = id,
                rates = rates
            };
            //return PartialView("_RatesHistory");
            return await Task.Run(() => PartialView("_RatesHistory", model));

        }

 
        public async Task<IActionResult> ExportHistoryToExcel(int id)
        {
            var rates = await _ratesService.GetCurencyActualMonthRatesAsync(id);

            var fileContent = await _ratesService.PrepareFileContent(rates, id);
            

            if(fileContent == null || fileContent.Length == 0)
            {
                return NotFound();
            }

            return await Task.Run(() => File(
                fileContents: fileContent,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "RatesHistory.xlsx"
                ));


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
