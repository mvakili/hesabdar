using Hesabdar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Hesabdar.Controllers
{
    public partial class DealController
    {
        // GET: api/Deal
        [HttpGet]
        public IActionResult Deals([FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            var deals = _context.Deal.Include("Seller").Include("Buyer").Include(i => i.DealPrice).Include(i => i.DealPayment).OrderBy(sort).PageResult(page, perPage);
            return Ok(deals);
        }

        [HttpGet("Dealer/{id}")]
        public ActionResult<PagedResult<Deal>> GetDealsOfDealer([FromRoute] int id, [FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
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
    }
}
