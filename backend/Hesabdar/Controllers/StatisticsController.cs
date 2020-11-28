using Hesabdar.Helpers;
using Hesabdar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Hesabdar.Controllers
{
    [Produces("application/json")]
    [Route("api/Statistics")]
    public partial class StatisticsController : Controller
    {
        private readonly HesabdarContext _context;

        public StatisticsController(HesabdarContext context)
        {
            _context = context;
        }

        [ExcludeFromCodeCoverage]
        [HttpGet("PurchaseAndSale/Price/OverDate/Dealer/{id}")]
        [HttpGet("PurchaseAndSale/Price/OverDate")]
        public async Task<IActionResult> GetPurchaseAndSalePriceOverDate([FromRoute] int? id, [FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null)
        {
            var startDate = start ?? DateTime.Now.Date;
            var endDate = end ?? DateTime.Now.Date;
            if (id == null)
            {
                id = 1;
            }

            var dates = Enumerable.Range(0, (int)(endDate - startDate).TotalDays + 1)
                                  .Select(x => startDate.AddDays(x))
                                  .ToList();

            var purchasesAmount = _context.Payment
                .Where(u => u.Method == Models.Enums.PaymentMethod.DealPrice)
                .Where(u => u.PayeeId == id)
                .Where(u => u.PayDate.Date >= startDate.Date && u.PayDate.Date <= endDate)
                .GroupBy(u => u.PayDate.Date)
                .Select(u => new
                {
                    Date = u.Key,
                    Amount = u.Sum(p => p.Amount)
                });

            var salesAmount = _context.Payment
                .Where(u => u.Method == Models.Enums.PaymentMethod.DealPrice)
                .Where(u => u.PayerId == id)
                .Where(u => u.PayDate.Date >= startDate.Date && u.PayDate.Date <= endDate.Date)
                .GroupBy(u => u.PayDate.Date)
                .Select(u => new
                {
                    Date = u.Key,
                    Amount = u.Sum(p => p.Amount)
                });

            var result = (from d in dates
                          join pa in purchasesAmount on d.Date equals pa.Date into pai
                          from pa in pai.DefaultIfEmpty()
                          join sa in salesAmount on d.Date equals sa.Date into sai
                          from sa in sai.DefaultIfEmpty()
                          select new
                          {
                              d.Date,
                              PurchasesAmount = pa?.Amount ?? 0,
                              SalesAmount = sa?.Amount ?? 0
                          });

            return Ok(result);
        }


    }
}