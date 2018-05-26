using System;
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
    [Route("api/Material")]
    public class MaterialController : Controller
    {
        private readonly HesabdarContext _context;

        public MaterialController(HesabdarContext context)
        {
            _context = context;
        }

        // GET: api/Material
        [HttpGet]
        public IActionResult GetMaterials([FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            var materials = _context.Material.OrderBy(sort).PageResult(page, perPage);
            return Ok(materials);
        }

        [HttpGet("Suggest")]
        [HttpGet("Suggest/{text}")]
        public IActionResult GetSuggestedMaterials([FromRoute] string text = "")
        {
            text = text.ToLower();

            var materials = _context.Material
                .Where(u => u.Name.ToLower().Contains(text) || u.Barcode == text)
                .Take(10);
            return Ok(materials);
        }

        // GET: api/Material/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaterial([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var material = await _context.Material.SingleOrDefaultAsync(m => m.Id == id);

            if (material == null)
            {
                return NotFound();
            }

            return Ok(material);
        }

        // PUT: api/Material/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterial([FromRoute] int id, [FromBody] Material material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != material.Id)
            {
                return BadRequest();
            }

            _context.Entry(material).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
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

        // POST: api/Material
        [HttpPost]
        public async Task<IActionResult> PostMaterial([FromBody] Material material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Material.Add(material);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaterial", new { id = material.Id }, material);
        }

        // DELETE: api/Material/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var material = await _context.Material.SingleOrDefaultAsync(m => m.Id == id);
            if (material == null)
            {
                return NotFound();
            }

            _context.Material.Remove(material);
            await _context.SaveChangesAsync();

            return Ok(material);
        }

        private bool MaterialExists(int id)
        {
            return _context.Material.Any(e => e.Id == id);
        }
    }
}