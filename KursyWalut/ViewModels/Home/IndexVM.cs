using KursyWalut.Enums;
using KursyWalut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KursyWalut.ViewModels.Home
{
    public class IndexVM
    {
        public List<Rate> Rates { get; set; }
        public CurrencyCodes CurrencyCodes { get; set; }
        public List<AverageTable> AvarageRates { get; set; }
    }
}
