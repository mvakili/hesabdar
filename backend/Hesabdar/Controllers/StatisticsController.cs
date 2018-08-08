using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hesabdar.Models;

namespace Hesabdar.Controllers
{
    [Produces("application/json")]
    [Route("api/Statistics")]
    public class StatisticsController : Controller
    {
        private readonly HesabdarContext _context;

        public StatisticsController(HesabdarContext context)
        {
            _context = context;
        }
        [HttpGet("General/Dealer/{id}")]
        [HttpGet("General")]
        public async Task<IActionResult> GetGeneral([FromRoute] int? id)
        {
            if (id == null)
            {
                id = 1;
            }
            var todaySaleInvoicesCount = _context.Deal.Where(u => u.DealTime.Date == DateTime.Today && u.SellerId == id)
                .CountAsync();
            var todayPurchaseInvoicesCount = _context.Deal.Where(u => u.DealTime.Date == DateTime.Today && u.BuyerId == id)
                .CountAsync();
            var todaySalesPrice = _context.Payment.Where(u => u.Method == Models.Enums.PaymentMethod.DealPrice)
                .Where(u => u.PayDate.Date == DateTime.Today && u.PayerId == id)
                .Select(u => u.Amount)
                .DefaultIfEmpty(0)
                .SumAsync();
            var todayPurchasePrice = _context.Payment.Where(u => u.Method == Models.Enums.PaymentMethod.DealPrice)
                .Where(u => u.PayDate.Date == DateTime.Today && u.PayeeId == id)
                .Select(u => u.Amount)
                .DefaultIfEmpty(0)
                .SumAsync();


            return Ok(
                new
                {
                    todaySaleInvoicesCount = todaySaleInvoicesCount.Result,
                    todayPurchaseInvoicesCount = todayPurchaseInvoicesCount.Result,
                    todaySalesPrice = todaySalesPrice.Result,
                    todayPurchasePrice = todayPurchasePrice.Result
                }
            );
        }
        [HttpGet("Sales/Price/TwoWeeks/Dealer/{id}")]
        [HttpGet("Sales/Price/TwoWeeks")]
        public async Task<IActionResult> GetTwoWeeksSalesPrice([FromRoute] int? id)
        {
            if (id == null)
            {
                id = 1;
            }
            var startDate = DateTime.Now.AddDays(-14);
            var endDate = DateTime.Now;
            var today = DateTime.Today;

            var dates = Enumerable.Range(0, (int)(endDate - startDate).TotalDays)
                                  .Select(x => startDate.AddDays(x + 1))
                                  .ToList();

            var amounts = _context.Payment
                .Where(u => u.Method == Models.Enums.PaymentMethod.DealPrice)
                .Where(u => u.PayerId == id)
                .Where(u => u.PayDate.Date > startDate.Date && u.PayDate.Date <= today)
                .GroupBy(u => u.PayDate.Date)
                .Select(u => new
                {
                    Date = u.Key,
                    Amount = u.Sum(p => p.Amount)
                });

            var result = (from d in dates
             join a in amounts on d.Date equals a.Date into ai
             from a in ai.DefaultIfEmpty()
             select new
             {
                 d.Date,
                 Amount = a?.Amount ?? 0
             });
            
            try
            {
                var tt = result.ToList();
            } catch (Exception ex)
            {

            }
            return Ok(result);
        }

        [HttpGet("Purchases/Price/TwoWeeks/Dealer/{id}")]
        [HttpGet("Purchases/Price/TwoWeeks")]
        public async Task<IActionResult> GetTwoWeeksPurchasesPrice([FromRoute] int? id)
        {
            if (id == null)
            {
                id = 1;
            }
            var startDate = DateTime.Now.AddDays(-14);
            var endDate = DateTime.Now;
            var today = DateTime.Today;

            var dates = Enumerable.Range(0, (int)(endDate - startDate).TotalDays)
                                  .Select(x => startDate.AddDays(x + 1))
                                  .ToList();

            var amounts = _context.Payment
                .Where(u => u.Method == Models.Enums.PaymentMethod.DealPrice)
                .Where(u => u.PayeeId == id)
                .Where(u => u.PayDate.Date > startDate.Date && u.PayDate.Date <= today)
                .GroupBy(u => u.PayDate.Date)
                .Select(u => new
                {
                    Date = u.Key,
                    Amount = u.Sum(p => p.Amount)
                });

            var result = (from d in dates
                          join a in amounts on d.Date equals a.Date into ai
                          from a in ai.DefaultIfEmpty()
                          select new
                          {
                              d.Date,
                              Amount = a?.Amount ?? 0
                          });

            return Ok(result);
        }

        [HttpGet("PurchaseAndSale/Price/Weekly/Dealer/{id}")]
        [HttpGet("PurchaseAndSale/Price/Weekly")]
        public async Task<IActionResult> GetWeeklyPurchaseAndSalePrice([FromRoute] int? id)
        {
            if (id == null)
            {
                id = 1;
            }
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;
            var today = DateTime.Today;

            var dates = Enumerable.Range(0, (int)(endDate - startDate).TotalDays)
                                  .Select(x => startDate.AddDays(x + 1))
                                  .ToList();

            var purchasesAmount = _context.Payment
                .Where(u => u.Method == Models.Enums.PaymentMethod.DealPrice)
                .Where(u => u.PayeeId == id)
                .Where(u => u.PayDate.Date > startDate.Date && u.PayDate.Date <= today)
                .GroupBy(u => u.PayDate.Date)
                .Select(u => new
                {
                    Date = u.Key,
                    Amount = u.Sum(p => p.Amount)
                });

            var salesAmount = _context.Payment
                .Where(u => u.Method == Models.Enums.PaymentMethod.DealPrice)
                .Where(u => u.PayerId == id)
                .Where(u => u.PayDate.Date > startDate.Date && u.PayDate.Date <= endDate.Date)
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

        [HttpGet("PurchaseAndSale/Price/Monthly/Dealer/{id}")]
        [HttpGet("PurchaseAndSale/Price/Monthly")]
        public async Task<IActionResult> GetMonthlyPurchaseAndSalePrice([FromRoute] int? id)
        {
            if (id == null)
            {
                id = 1;
            }
            var startDate = DateTime.Now.AddDays(-30);
            var endDate = DateTime.Now;
            var today = DateTime.Today;

            var dates = Enumerable.Range(0, (int)(endDate - startDate).TotalDays)
                                  .Select(x => startDate.AddDays(x + 1))
                                  .ToList();

            var purchasesAmount = _context.Payment
                .Where(u => u.Method == Models.Enums.PaymentMethod.DealPrice)
                .Where(u => u.PayeeId == id)
                .Where(u => u.PayDate.Date > startDate.Date && u.PayDate.Date <= today)
                .GroupBy(u => u.PayDate.Date)
                .Select(u => new
                {
                    Date = u.Key,
                    Amount = u.Sum(p => p.Amount)
                });

            var salesAmount = _context.Payment
                .Where(u => u.Method == Models.Enums.PaymentMethod.DealPrice)
                .Where(u => u.PayerId == id)
                .Where(u => u.PayDate.Date > startDate.Date && u.PayDate.Date <= today)
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