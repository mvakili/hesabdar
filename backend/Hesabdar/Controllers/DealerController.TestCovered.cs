using Hesabdar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Hesabdar.Controllers
{

    public partial class DealerController : Controller
    {
        // GET: api/Dealer
        [HttpGet]
        public IActionResult Dealers([FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {

            var incomes = _context.Payment.Where(u => u.Paid).Where(u => u.PayerId == 1).GroupBy(u => u.PayeeId).Select(u => new { DealerId = u.Key, Amount = u.Select(i => i.Amount).DefaultIfEmpty(0).Sum() });
            var expenses = _context.Payment.Where(u => u.Paid).Where(u => u.PayeeId == 1).GroupBy(u => u.PayerId).Select(u => new { DealerId = u.Key, Amount = u.Select(i => i.Amount).DefaultIfEmpty(0).Sum() });

            var dealers = (
                from d in _context.Dealer
                join i in incomes on d.Id equals i.DealerId into iIncome
                from income in iIncome.DefaultIfEmpty()
                join e in expenses on d.Id equals e.DealerId into iExpenses
                from expense in iExpenses.DefaultIfEmpty()
                where d.Id != 1
                select new Dealer
                {
                    Address = d.Address,
                    Id = d.Id,
                    Name = d.Name,
                    PhoneNumber = d.PhoneNumber,
                    Timestamp = d.Timestamp,
                    Balance = (expense != null ? expense.Amount : 0) - (income != null ? income.Amount : 0)
                }
            ).OrderBy(sort).PageResult(page, perPage);
            return Ok(dealers);
        }
        [HttpGet("Suggest")]
        [HttpGet("Suggest/{text}")]
        public IActionResult GetSuggestedDealers([FromRoute] string text = "")
        {
            text = text.ToLower();
            var results = new List<Dealer>();
            foreach (var item in _context.Dealer)
            {
                if (item.Id != 1 && item.Name.ToLower().Contains(text))
                {
                    results.Add(item);
                }
            }
            var dealers = results.Take(10);
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

            var incomes = _context.Payment.Where(u => u.Paid).Where(u => u.PayerId == 1).GroupBy(u => u.PayeeId).Select(u => new { DealerId = u.Key, Amount = u.Select(i => i.Amount).DefaultIfEmpty(0).Sum() });
            var expenses = _context.Payment.Where(u => u.Paid).Where(u => u.PayeeId == 1).GroupBy(u => u.PayerId).Select(u => new { DealerId = u.Key, Amount = u.Select(i => i.Amount).DefaultIfEmpty(0).Sum() });

            var dealers = (
                from d in _context.Dealer
                join i in incomes on d.Id equals i.DealerId into iIncome
                from income in iIncome.DefaultIfEmpty()
                join e in expenses on d.Id equals e.DealerId into iExpenses
                from expense in iExpenses.DefaultIfEmpty()
                select new Dealer
                {
                    Address = d.Address,
                    Id = d.Id,
                    Name = d.Name,
                    PhoneNumber = d.PhoneNumber,
                    Timestamp = d.Timestamp,
                    Balance = (expense != null ? expense.Amount : 0) - (income != null ? income.Amount : 0)
                }
            );

            var dealer = await dealers.SingleOrDefaultAsync(m => m.Id == id);

            if (dealer == null)
            {
                return NotFound();
            }

            return Ok(dealer);
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


    }
}