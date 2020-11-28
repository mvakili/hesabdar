using Hesabdar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Hesabdar.Controllers
{
    [Produces("application/json")]
    [Route("api/Deal")]
    public partial class DealController : Controller
    {
        private readonly HesabdarContext _context;

        public DealController(HesabdarContext context)
        {
            _context = context;
        }

        // GET: api/Deal/5
        [ExcludeFromCodeCoverage]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeal([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deal = await _context.Deal.Include("Seller").Include("Buyer").Include(i => i.DealPrice).Include(i => i.DealPayment).SingleOrDefaultAsync(m => m.Id == id);

            if (deal == null)
            {
                return NotFound();
            }

            return Ok(deal);
        }

        // PUT: api/Deal/5
        [ExcludeFromCodeCoverage]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeal([FromRoute] int id, [FromBody] Deal deal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deal.Id)
            {
                return BadRequest();
            }

            #region Deal Modify
            _context.Entry(deal).State = EntityState.Modified;
            #endregion

            #region Payments Modify
            if (deal.DealPayment.Method == Models.Enums.PaymentMethod.Cash && !deal.DealPayment.Paid)
            {
                deal.DealPayment.Amount = 0;
            }
            deal.DealPayment.PayerId = deal.BuyerId;
            deal.DealPayment.PayeeId = deal.SellerId;
            deal.DealPrice.PayerId = deal.SellerId;
            deal.DealPrice.PayeeId = deal.BuyerId;
            deal.DealPrice.PayDate = deal.DealTime;

            _context.Entry(deal.DealPayment).State = EntityState.Modified;
            _context.Entry(deal.DealPrice).State = EntityState.Modified;

            #endregion

            #region Deal Items Modify
            var newItems = deal.Items.ToList();
            var deletingItems = _context.DealItem.Where(u => u.DealId == id && !newItems.Select(i => i.Id).Contains(u.Id)).ToList();

            deletingItems.ForEach(i =>
            {
                _context.Entry(i).State = EntityState.Deleted;
            });

            newItems.ForEach(i =>
            {

                if (i.Id != 0)
                {
                    _context.Entry(i).State = EntityState.Modified;
                }
                else
                {
                    _context.Entry(i).State = EntityState.Added;
                }
            });


            #endregion
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!DealExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();

        }

        [HttpPost("Sale")]
        [ExcludeFromCodeCoverage]
        public async Task<IActionResult> PostSale([FromBody] Deal deal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            deal.SellerId = 1;
            deal.DealPrice.PayeeId = deal.BuyerId;
            deal.DealPrice.PayerId = 1;
            deal.DealPayment.PayeeId = 1;
            deal.DealPayment.PayerId = deal.BuyerId;
            return await PostDeal(deal);
        }
        [HttpPost("Purchase")]
        [ExcludeFromCodeCoverage]
        public async Task<IActionResult> PostPurchase([FromBody] Deal deal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            deal.BuyerId = 1;
            deal.DealPrice.PayeeId = 1;
            deal.DealPrice.PayerId = deal.SellerId;
            deal.DealPayment.PayeeId = deal.SellerId;
            deal.DealPayment.PayerId = 1;


            return await PostDeal(deal);
        }
        [ExcludeFromCodeCoverage]
        private async Task<IActionResult> PostDeal([FromBody] Deal deal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (deal.DealTime == DateTime.MinValue)
            {
                deal.DealTime = DateTime.Now;
            }
            if (deal.DealPayment.Method == Models.Enums.PaymentMethod.Cash && !deal.DealPayment.Paid)
            {
                deal.DealPayment.Amount = 0;
            }

            deal.DealPrice.PayDate = deal.DealTime;
            deal.DealPrice.Method = Models.Enums.PaymentMethod.DealPrice;
            deal.DealPrice.Paid = true;



            if (deal.DealPayment.PayDate == DateTime.MinValue)
            {
                deal.DealPayment.PayDate = DateTime.Now;
            }


            _context.Entry(deal).State = EntityState.Added;
            deal.Items.ToList().ForEach(i =>
            {
                _context.Entry(i).State = EntityState.Added;
            });
            _context.Entry(deal.DealPayment).State = EntityState.Added;
            _context.Entry(deal.DealPrice).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeal", new { id = deal.Id }, deal);
        }

        // DELETE: api/Deal/5
        [ExcludeFromCodeCoverage]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeal([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deal = await _context.Deal.SingleOrDefaultAsync(m => m.Id == id);
            if (deal == null)
            {
                return NotFound();
            }

            _context.Deal.Remove(deal);
            await _context.SaveChangesAsync();

            return Ok(deal);
        }
        [ExcludeFromCodeCoverage]
        private bool DealExists(int id)
        {
            return _context.Deal.Any(e => e.Id == id);
        }
    }
}