using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KursyWalut.Models
{
    public class Root
    {
        public string table { get; set; }
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public List<Rate> rates { get; set; }
    }
}
