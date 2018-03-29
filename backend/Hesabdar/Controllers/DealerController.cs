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
    [Route("api/Dealer")]
    public class DealerController : Controller
    {
        private readonly HesabdarContext _context;

        public DealerController(HesabdarContext context)
        {
            _context = context;
        }

        // GET: api/Dealer
        [HttpGet]
        public IActionResult Dealers([FromQuery] int page = 1, [FromQuery] string order = "id desc")
        {
            var dealers = _context.Dealer.OrderBy(order).PageResult(page, 10);
            return Ok(dealers);
        }

        // GET: api/Dealer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDealer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dealer = await _context.Dealer.SingleOrDefaultAsync(m => m.Id == id);

            if (dealer == null)
            {
                return NotFound();
            }

            return Ok(dealer);
        }

        // PUT: api/Dealer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDealer([FromRoute] int id, [FromBody] Dealer dealer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dealer.Id)
            {
                return BadRequest();
            }

            _context.Entry(dealer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DealerExists(id))
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

        // POST: api/Dealer
        [HttpPost]
        public async Task<IActionResult> PostDealer([FromBody] Dealer dealer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Dealer.Add(dealer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDealer", new { id = dealer.Id }, dealer);
        }

        // DELETE: api/Dealer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDealer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dealer = await _context.Dealer.SingleOrDefaultAsync(m => m.Id == id);
            if (dealer == null)
            {
                return NotFound();
            }

            _context.Dealer.Remove(dealer);
            await _context.SaveChangesAsync();

            return Ok(dealer);
        }

        private bool DealerExists(int id)
        {
            return _context.Dealer.Any(e => e.Id == id);
        }
    }
}