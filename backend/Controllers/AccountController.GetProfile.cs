using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Models.Data.Account;
using api.Providers;
using api.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static api.Services.AccountService;

namespace api.Controllers
{
    public partial class AccountController : HesabdarController
    {
        public class GetProfileInput {
           public string Username { get; set; }
        }

        [HttpPost]
        [Route("[action]")]
        public ApiResult<ProfileResult> GetProfile([FromBody] GetProfileInput input)
        {
            var result =  new Job<ProfileResult>
            {
                Permission = PermissionLevel.Spectrator,
                Controller = this
            };

            return result.Run(
            (ref ApiResult<ProfileResult> res) =>
            {
                res = result.UseService<AccountService>().GetProfile(input.Username);
            }
            );
        }
    }
}
