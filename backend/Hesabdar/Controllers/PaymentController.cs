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
    [Route("api/Payment")]
    public partial class PaymentController : Controller
    {
        private readonly HesabdarContext _context;
        [ExcludeFromCodeCoverage]
        public PaymentController(HesabdarContext context)
        {
            _context = context;
        }
        [ExcludeFromCodeCoverage]
        [HttpGet]
        [HttpGet("Dealer/{id}")]
        [HttpGet("Dealer/{id}/SecondParty/{secondPartyId}")]
        [HttpGet("SecondParty/{secondPartyId}")]
        public IActionResult GetPaymentsOfDealer([FromRoute] int? id = null, [FromRoute] int? secondPartyId = null, [FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "payDate asc", [FromQuery] string filter = "")
        {
            id = id ?? 1;

            var paymentsQuery = PaymentResult.Where(u => u.PayeeId == id || u.PayerId == id);
            if (secondPartyId != null)
            {
                paymentsQuery = paymentsQuery.Where(u => u.PayeeId == secondPartyId || u.PayerId == secondPartyId);
            }
            var payments = paymentsQuery.OrderBy(sort).PageResult(page, perPage);
            return Ok(payments);
        }

        [ExcludeFromCodeCoverage]
        [HttpGet("Dealer/Incomes/{id}")]
        public IActionResult GetIncomesOfDealer([FromRoute] int id, [FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            var payments = PaymentResult.Where(u => u.PayeeId == id).OrderBy(u => u.PayDate).PageResult(page, perPage);
            return Ok(payments);
        }
        [ExcludeFromCodeCoverage]
        [HttpGet("Dealer/Expenses/{id}")]
        public IActionResult GetExpensesOfDealer([FromRoute] int id, [FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {


            var payments = PaymentResult.Where(u => u.PayerId == id).OrderBy(u => u.PayDate).PageResult(page, perPage);
            return Ok(payments);
        }
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payment = await PaymentResult.SingleOrDefaultAsync(m => m.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        // PUT: api/Payment/5
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.Id == id);
        }
        [ExcludeFromCodeCoverage]
        public IQueryable<PaymentModel> PaymentResult => (from p in _context.Payment
                                                          join payer in _context.Dealer on p.PayerId equals payer.Id
                                                          join payee in _context.Dealer on p.PayeeId equals payee.Id
                                                          join dpayment in _context.Deal on p.Id equals dpayment.DealPaymentId into iDealPayment
                                                          from dealPayment in iDealPayment.DefaultIfEmpty()
                                                          join dprice in _context.Deal on p.Id equals dprice.DealPriceId into iDealPrice
                                                          from dealPrice in iDealPrice.DefaultIfEmpty()
                                                          select new PaymentModel
                                                          {
                                                              Amount = p.Amount,
                                                              Id = p.Id,
                                                              Method = p.Method,
                                                              Paid = p.Paid,
                                                              PayDate = p.PayDate,
                                                              PayeeId = p.PayeeId,
                                                              PayerId = p.PayerId,
                                                              Timestamp = p.Timestamp,
                                                              Payee = payee,
                                                              Payer = payer,
                                                              IsDealPrice = dealPrice != null,
                                                              IsDealPayment = dealPayment != null,
                                                              Deal = dealPrice ?? dealPayment
                                                          }
        );
    }
}