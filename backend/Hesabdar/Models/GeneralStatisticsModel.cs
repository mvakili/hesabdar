using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hesabdar.Models
{
    public class GeneraStatisticsModel
    {
        public int todaySaleInvoicesCount { get; set; }
        public int todayPurchaseInvoicesCount { get; set; }
        public decimal todaySalesPrice { get; set; }
        public decimal todayPurchasePrice { get; set; }
    }
}
