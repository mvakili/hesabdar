using Hesabdar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Hesabdar.Controllers
{
    public partial class MaterialController
    {
        // GET: api/Material
        [HttpGet]
        public IActionResult GetMaterials([FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            var materials = _context.Material.OrderBy(sort).PageResult(page, perPage);
            return Ok(materials);
        }




    }
}
