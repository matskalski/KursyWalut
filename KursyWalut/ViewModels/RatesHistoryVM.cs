using KursyWalut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KursyWalut.ViewModels
{
    public class RatesHistoryVM
    {
        public int currencyId { get; set; }
        public List<Rate> rates { get; set; }
    }
}
