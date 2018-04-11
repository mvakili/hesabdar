using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hesabdar.Models;

namespace Hesabdar.Controllers
{
    [Produces("application/json")]
    [Route("api/DealItem")]
    public class DealItemController : Controller
    {
        private readonly HesabdarContext _context;

        public DealItemController(HesabdarContext context)
        {
            _context = context;
        }

        // GET: api/DealItem
        [HttpGet("Deal/{id}")]
        public IEnumerable<DealItem> GetDealItemsOfDeal([FromRoute] int id)
        {
            return _context.DealItem.Include("Material").Where( u => u.DealId == id);
        }

        // GET: api/DealItem/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDealItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dealItem = await _context.DealItem.SingleOrDefaultAsync(m => m.Id == id);

            if (dealItem == null)
            {
                return NotFound();
            }

            return Ok(dealItem);
        }

        // PUT: api/DealItem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDealItem([FromRoute] int id, [FromBody] DealItem dealItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dealItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(dealItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DealItemExists(id))
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

        // POST: api/DealItem
        [HttpPost]
        public async Task<IActionResult> PostDealItem([FromBody] DealItem dealItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.DealItem.Add(dealItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDealItem", new { id = dealItem.Id }, dealItem);
        }

        // DELETE: api/DealItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDealItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dealItem = await _context.DealItem.SingleOrDefaultAsync(m => m.Id == id);
            if (dealItem == null)
            {
                return NotFound();
            }

            _context.DealItem.Remove(dealItem);
            await _context.SaveChangesAsync();

            return Ok(dealItem);
        }

        private bool DealItemExists(int id)
        {
            return _context.DealItem.Any(e => e.Id == id);
        }
    }
}