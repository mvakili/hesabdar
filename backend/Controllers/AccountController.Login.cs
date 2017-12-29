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
    public partial class AccountController : HesabdarController
    {
       public class LoginInput {
           public string Username { get; set; }
           public string Password {get; set;}
       }

        [HttpPost]
        [Route("[action]")]
        public ApiResult Login([FromBody] LoginInput input)
        {
            var result =  new Job
            {
                Controller = this
            };

            return result.Run(
            (ref ApiResult res) =>
            {
                res = result.UseService<AccountService>().Login(input.Username, input.Password);
            }
            );
        }
    }
}
