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
    [Route("api/DealItem")]
    public partial class DealItemController : Controller
    {
        private readonly HesabdarContext _context;
        public DealItemController(HesabdarContext context)
        {
            _context = context;
        }

       
       
        
        
        [ExcludeFromCodeCoverage]
        private bool DealItemExists(int id)
        {
            return _context.DealItem.Any(e => e.Id == id);
        }
        [ExcludeFromCodeCoverage]
        [HttpGet("LastSalePrice/{materialId}")]
        public IActionResult GetLastSalePrice(int materialId)
        {
            var price = (from di in _context.DealItem
                         join d in _context.Deal on di.DealId equals d.Id
                         where d.SellerId == 1
                         where di.MaterialId == materialId
                         orderby d.DealTime descending
                         orderby di.Id
                         select di.PricePerOne
                    ).DefaultIfEmpty(0).FirstOrDefault();

            return Ok(price);
        }
        [ExcludeFromCodeCoverage]
        [HttpGet("LastPurchasePrice/{materialId}")]
        public IActionResult GetLastPurchasePrice(int materialId)
        {
            var price = (from di in _context.DealItem
                         join d in _context.Deal on di.DealId equals d.Id
                         where d.BuyerId == 1
                         where di.MaterialId == materialId
                         orderby d.DealTime descending
                         orderby di.Id
                         select di.PricePerOne
                    ).DefaultIfEmpty(0).FirstOrDefault();

            return Ok(price);
        }

       

    }
}