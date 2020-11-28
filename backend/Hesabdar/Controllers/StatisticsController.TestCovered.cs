using Hesabdar.Helpers;
using Hesabdar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Hesabdar.Controllers
{

    public partial class StatisticsController {
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
                new GeneraStatisticsModel
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
                          select new AmountOverDateModel
                          {
                              Date = d.Date,
                              Amount = a?.Amount ?? 0
                          });

            try
            {
                var tt = result.ToList();
            }
            catch (Exception ex)
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
                          select new AmountOverDateModel
                          {
                              Date = d.Date,
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
                          select new DatePurchaseAndSalePriceModel
                          {
                              Date = d.Date,
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

            List<DateTime> dates = DatetimeHelper.GenerateDatesInRange(startDate, endDate);

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
                          select new DatePurchaseAndSalePriceModel
                          {
                              Date = d.Date,
                              PurchasesAmount = pa?.Amount ?? 0,
                              SalesAmount = sa?.Amount ?? 0
                          });

            return Ok(result);
        }
        [HttpGet("Material/OverTime/{materialId}")]
        [HttpGet("Material/OverTime/{materialId}/{dealerId}")]
        public async Task<IActionResult> GetMaterialAmountOverTime([FromRoute] int materialId, [FromRoute] int? dealerId = null, [FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null)
        {
            var startDate = start ?? DateTime.Now.Date;
            var endDate = end ?? DateTime.Now.Date;
            dealerId = dealerId ?? 1;
            var beforeAmount = (
                from DealItem in _context.DealItem
                join Deal in _context.Deal on DealItem.DealId equals Deal.Id
                where DealItem.MaterialId == materialId
                where Deal.DealTime.Date < startDate.Date
                where Deal.SellerId == dealerId || Deal.BuyerId == dealerId
                let coef = Deal.SellerId == dealerId ? -1 : 1
                select DealItem.Quantity * coef
            ).DefaultIfEmpty(0).Sum();

            var amounts = (
                from DealItem in _context.DealItem
                join Deal in _context.Deal on DealItem.DealId equals Deal.Id
                where DealItem.MaterialId == materialId
                where Deal.DealTime.Date >= startDate.Date && Deal.DealTime.Date <= endDate
                where Deal.SellerId == dealerId || Deal.BuyerId == dealerId
                let coef = Deal.SellerId == dealerId ? -1 : 1
                group new { Deal = Deal, DealItem = DealItem, coef = coef } by Deal.DealTime.Date into gg
                select new AmountOverDateModel
                {
                    Date = gg.Key.Date,
                    Amount = gg.Select(u => new { u.DealItem, u.coef }).Select(u => u.DealItem.Quantity * u.coef).DefaultIfEmpty(0).Sum()
                }
            ).ToList();

            var dates = Enumerable.Range(0, (int)(endDate - startDate).TotalDays + 1)
                                  .Select(x => startDate.AddDays(x))
                                  .ToList();

            amounts = (from date in dates
                       join a in amounts on date.Date equals a.Date into iAmount
                       from Amount in iAmount.DefaultIfEmpty()
                       select new AmountOverDateModel
                       {
                           Date = date.Date,
                           Amount = Amount?.Amount ?? 0
                       }).OrderBy(u => u.Date).ToList();

            amounts.ForEach(u =>
            {
                u.Amount += beforeAmount;
                beforeAmount = u.Amount;
            });

            return Ok(amounts);
        }


    }
}