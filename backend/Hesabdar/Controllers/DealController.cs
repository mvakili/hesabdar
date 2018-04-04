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
            var deals = _context.Deal.OrderBy(sort).PageResult(page, perPage);
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


            var deals = _context.Deal.Where(u => ((u.Buyer != null && u.Buyer.Id == id) || (u.Seller != null && u.Seller.Id == id)) && (u.Seller != null || u.Buyer != null)).OrderBy(sort).PageResult(page, perPage);
            return Ok(deals);
        }

        [HttpGet("Dealer/{id:int=1}/Sales")]
        public IActionResult GeDealesOfDealer([FromRoute] int id, [FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {

            var dealerExists = _context.Dealer.Any(u => u.Id == id);

            if (!dealerExists)
            {
                return BadRequest();
            }

            var deals = _context.Deal.Where(u => u.Seller != null && u.Seller.Id == id).OrderBy(sort).PageResult(page, perPage);
            return Ok(deals);
        }

        [HttpGet("Dealer/{id:int=1}/Purchases")]
        public IActionResult GetPurchasesOfDealer([FromRoute] int? id, [FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {

            var dealerExists = _context.Dealer.Any(u => u.Id == id);

            if (!dealerExists)
            {
                return BadRequest();
            }
            var deals = _context.Deal.Where(u => u.Buyer != null && u.Buyer.Id == id).OrderBy(sort).PageResult(page, perPage);
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

            var deal = await _context.Deal.SingleOrDefaultAsync(m => m.Id == id);

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

            _context.Entry(deal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
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