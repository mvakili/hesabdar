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
using static api.Services.MaterialService;

namespace api.Controllers
{
    public partial class MaterialController : HesabdarController
    {

        [HttpGet]
        [Route("[action]")]
        public ApiResult<List<GetMaterialsResult>> GetMaterials()
        {
            var result =  new Job<List<GetMaterialsResult>>
            {
                Permission = PermissionLevel.Spectrator,
                Controller = this
            };

            return result.Run(
            res =>
            {
                res = result.UseService<MaterialService>().GetMaterials();
            }
            );
        }
    }
}
