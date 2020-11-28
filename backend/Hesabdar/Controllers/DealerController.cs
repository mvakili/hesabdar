using Hesabdar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Hesabdar.Controllers
{
    [Produces("application/json")]
    [Route("api/Dealer")]
    public partial class DealerController : Controller
    {
        private readonly HesabdarContext _context;

        public DealerController(HesabdarContext context)
        {
            _context = context;
        }


        [ExcludeFromCodeCoverage]
        [HttpGet("Main")]
        public async Task<IActionResult> GetMainDealer()
        {
            return await GetDealer(id: 1);
        }

        // PUT: api/Dealer/5
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
        [HttpPut("Main")]
        public async Task<IActionResult> PutMainDealer([FromBody] Dealer dealer)
        {
            return await PutDealer(1, dealer);
        }

        // POST: api/Dealer
        [ExcludeFromCodeCoverage]
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

        [ExcludeFromCodeCoverage]
        private bool DealerExists(int id)
        {
            return _context.Dealer.Any(e => e.Id == id);
        }
    }
}