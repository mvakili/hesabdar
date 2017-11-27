using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Providers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    public partial class AccountController : Controller
    {
       
       [HttpPost]
        public ApiResult<bool> AmILoggedIn()
        {

            return new Job<bool>
            {
                Do = result =>
                {
                    result.Data = true;
                },
                Authorized = false
            }.Run();

        }
    }
}
