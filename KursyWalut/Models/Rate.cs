using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KursyWalut.Models
{
    public class Rate
    {
        public string effectiveDate { get; set; }
        public string code { get; set; }
        public double mid { get; set; }
    }
}
