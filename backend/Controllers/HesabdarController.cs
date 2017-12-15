using api.Providers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public abstract class HesabdarController : Controller
    {
        public ModuleContainer ModuleContainer {get; set;}
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            this.ModuleContainer = new ModuleContainer() {
                Session = context.HttpContext.Session
            };
        }
    }
}