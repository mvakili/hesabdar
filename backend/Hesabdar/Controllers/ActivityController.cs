using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hesabdar.Models;
using Microsoft.AspNetCore.Identity;

namespace Hesabdar.Controllers
{
    [Produces("application/json")]
    [Route("api/Activity")]
    public class ActivityController : Controller
    {
        private readonly HesabdarContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public ActivityController(UserManager<IdentityUser> userManager, HesabdarContext context)
        {
            _context = context;
            this.userManager = userManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <remarks>api/Activity?page=1&&order=id desc</remarks>
        /// <remarks>api/Activity?page=1&&order=id desc</remarks>
        [HttpGet]
        public IActionResult GetActivities([FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            var activities = _context.Activity.OrderBy(sort).PageResult(page, perPage);
            return Ok(activities);
        }

        // GET: api/Activity/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activity = await _context.Activity.SingleOrDefaultAsync(m => m.Id == id);

            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }
        // GET: api/Activity/User/demo?page=1&order=id desc
        [HttpGet("User/{username}")]
        public async Task<IActionResult> GetUserActivities([FromRoute] string username, [FromQuery] int page = 1, [FromQuery] string order = "id desc")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest();
            }

            var activities = _context.Activity.Where(u => u.UserName == username).OrderBy(order).PageResult(page, 10);


            return Ok(activities);
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.Id == id);
        }
    }
}