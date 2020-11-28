using Hesabdar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Hesabdar.Controllers
{
   
    public partial class DealItemController : Controller
    {
        // GET: api/DealItem
        [HttpGet("Deal/{id}")]
        public IActionResult GetDealItemsOfDeal([FromRoute] int id)
        {
            return Ok(_context.DealItem.Include("Material").Where(u => u.DealId == id).OrderBy("id"));
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
        [HttpGet("Material/{materialId}")]
        public IActionResult DealItemsOfMaterial([FromRoute] int materialId, [FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "deal.dealTime desc", [FromQuery] string filter = "")
        {
            var dealItems = (from di in _context.DealItem
                             join d in _context.Deal on di.DealId equals d.Id
                             select new DealItemOfMaterialModel
                             {
                                 Id = di.Id,
                                 PricePerOne = di.PricePerOne,
                                 Quantity = di.Quantity,
                                 Timestamp = di.Timestamp,
                                 DealId = di.DealId,
                                 MaterialId = di.MaterialId,
                                 deal = new DealItemOfMaterialModel.DealerModel
                                 {
                                     Id = d.Id,
                                     SellerId = d.SellerId,
                                     BuyerId = d.BuyerId,
                                     DealTime = d.DealTime,
                                     Timestamp = d.Timestamp,
                                     DealPriceId = d.DealPriceId,
                                     DealPaymentId = d.DealPaymentId,
                                     Buyer = d.Buyer,
                                     Seller = d.Seller
                                 }
                             }).Where(u => u.MaterialId == materialId).OrderBy(sort).PageResult(page, perPage);
            return Ok(dealItems);
        }


    }
}