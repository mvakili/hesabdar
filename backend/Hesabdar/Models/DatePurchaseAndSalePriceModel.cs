using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hesabdar.Models
{
    public class DatePurchaseAndSalePriceModel
    {
        public DateTime Date { get; set; }
        public decimal PurchasesAmount { get; set; }
        public decimal SalesAmount { get; set; }
    }
}
