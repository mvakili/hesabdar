using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Providers;
using api.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    public partial class AccountController : HesabdarController
    {
       
       [HttpPost]
        public ApiResult<bool> AmILoggedIn()
        {
            var result =  new Job<bool>
            {
                Authorized = false,
                Controller = this
            };

            return result.Run(
            res =>
            {
                res = result.UseService<AccountService>().AmILoggedIn();
            }
            );
        }
    }
}
