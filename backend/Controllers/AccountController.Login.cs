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
       public class LoginInput {
           public string Username { get; set; }
           public string Password {get; set;}
       }

       [HttpPost]
        public ApiResult<bool> Login([FromBody] LoginInput input)
        {
            var result =  new Job<bool>
            {
                Controller = this
            };

            return result.Run(
            res =>
            {
                res = result.UseService<AccountService>().Login(input.Username, input.Password);
            }
            );
        }
    }
}
