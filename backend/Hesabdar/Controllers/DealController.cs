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
    [Route("api/Deal")]
    public class DealController : Controller
    {
        private readonly HesabdarContext _context;

        public DealController(HesabdarContext context)
        {
            _context = context;
        }

        // GET: api/Deal
        [HttpGet]
        public IActionResult Deals([FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            var deals = _context.Deal.Include("Seller").Include("Buyer").Include(i => i.DealPrice).Include(i => i.DealPayment).OrderBy(sort).PageResult(page, perPage);
            return Ok(deals);
        }

        [HttpGet("Dealer/{id}")]
        public IActionResult GetDealsOfDealer([FromRoute] int id, [FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            var dealerExists = _context.Dealer.Any(u => u.Id == id);

            if (!dealerExists)
            {
                return BadRequest();
            }


            var deals = _context.Deal.Include("Seller").Include("Buyer").Include(i => i.DealPrice).Include(i => i.DealPayment).Where(u => ((u.Buyer != null && u.Buyer.Id == id) || (u.Seller != null && u.Seller.Id == id)) && (u.Seller != null || u.Buyer != null)).OrderBy(sort).PageResult(page, perPage);
            return Ok(deals);
        }
        [HttpGet("Dealer/Sales")]

        [HttpGet("Dealer/Sales/{id}")]
        public IActionResult GetSalesOfDealer([FromRoute] int? id, [FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            if (id == null)
            {
                id = 1;
            }
            var dealerExists = _context.Dealer.Any(u => u.Id == id);

            if (!dealerExists)
            {
                return BadRequest();
            }

            var deals = _context.Deal.Include("Seller").Include("Buyer").Include(i => i.DealPrice).Include(i => i.DealPayment).Where(u => u.Seller != null && u.Seller.Id == id).OrderBy(sort).PageResult(page, perPage);
            return Ok(deals);
        }
        [HttpGet("Dealer/Purchases")]
        [HttpGet("Dealer/Purchases/{id}")]
        public IActionResult GetPurchasesOfDealer([FromRoute] int? id = null, [FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            if (id == null)
            {
                id = 1;
            }
            var dealerExists = _context.Dealer.Any(u => u.Id == id);

            if (!dealerExists)
            {
                return BadRequest();
            }
            var deals = _context.Deal.Include("Seller").Include("Buyer").Include(i => i.DealPrice).Include(i => i.DealPayment).Where(u => u.Buyer != null && u.Buyer.Id == id).OrderBy(sort).PageResult(page, perPage);
            return Ok(deals);
        }

        // GET: api/Deal/5
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
            deal.DealPayment.PayerId = deal.BuyerId;
            deal.DealPayment.PayeeId = deal.SellerId;
            deal.DealPrice.PayerId = deal.SellerId;
            deal.DealPrice.PayeeId = deal.BuyerId;

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
                } else
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

        // POST: api/Deal
        [HttpPost]
        public async Task<IActionResult> PostDeal([FromBody] Deal deal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Deal.Add(deal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeal", new { id = deal.Id }, deal);
        }

        // DELETE: api/Deal/5
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

        private bool DealExists(int id)
        {
            return _context.Deal.Any(e => e.Id == id);
        }
    }
}