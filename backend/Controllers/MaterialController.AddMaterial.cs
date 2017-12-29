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
    public partial class MaterialController : HesabdarController
    {
        public class AddMaterialInput {
           public string Name { get; set; }
        }

        [HttpPost]
        [Route("[action]")]
        public ApiResult<int> AddMaterial([FromBody] AddMaterialInput input)
        {
            var result =  new Job<int>
            {
                Permission = PermissionLevel.Spectrator,
                Controller = this
            };

            return result.Run(
            res =>
            {
                res = result.UseService<MaterialService>().AddMaterial(input.Name);
            }
            );
        }
    }
}
