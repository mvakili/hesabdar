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
    [Route("api/Payment")]
    public class PaymentController : Controller
    {
        private readonly HesabdarContext _context;

        public PaymentController(HesabdarContext context)
        {
            _context = context;
        }

        // GET: api/Payment
        [HttpGet]
        public IActionResult GetPayments([FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            var materials = _context.Payment.Where(u => u.Amount > 0).OrderBy(sort).PageResult(page, perPage);
            return Ok(materials);
        }

        [HttpGet("Dealer/{id}")]
        public IActionResult GetPaymentsOfDealer([FromRoute] int id, [FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            var payments = _context.Payment.Where(u => u.Amount > 0).Where(u => u.PayeeId == id || u.PayerId == id);
            return Ok(payments);
        }

        [HttpGet("Dealer/Incomes/{id}")]
        public IActionResult GetIncomesOfDealer([FromRoute] int id, [FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            var incomes = _context.Dealer.Where(u => u.Id == id).Include(u => u.Incomes.Where(i => i.Amount > 0)).Select(u => u.Incomes);
            return Ok(incomes);
        }

        [HttpGet("Dealer/Expenses/{id}")]
        public IActionResult GetExpensesOfDealer([FromRoute] int id, [FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            var expenses = _context.Dealer.Where(u => u.Id == id).Include(u => u.Expenses.Where(e => e.Amount > 0)).Select(u => u.Expenses);
            return Ok(expenses);
        }

        [HttpGet("Deal/Price/{id}")]
        public async Task<IActionResult> GetPriceOfDeal([FromRoute] int id)
        {
            var payment = _context.Deal.Include(u => u.DealPrice).SingleOrDefault(u => u.Id == id).DealPrice;
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [HttpGet("Deal/{id}")]
        public async Task<IActionResult> GetPaymentOfDeal([FromRoute] int id)
        {
            var payment = _context.Deal.Include(u => u.DealPayment).SingleOrDefault(u => u.Id == id).DealPayment;
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        // GET: api/Payment/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payment = await _context.Payment.SingleOrDefaultAsync(m => m.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        // PUT: api/Payment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment([FromRoute] int id, [FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payment.Id)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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

        // POST: api/Payment
        [HttpPost]
        public async Task<IActionResult> PostPayment([FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Payment.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);
        }

        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payment = await _context.Payment.SingleOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();

            return Ok(payment);
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.Id == id);
        }
    }
}