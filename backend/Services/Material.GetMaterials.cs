using System.Linq;
using api.DataContext;
using api.Models;
using Microsoft.AspNetCore.Http;
using api.Models.Data.Materials;
using api.Providers;
using System.Collections.Generic;

namespace api.Services
{
    public partial class MaterialService : BaseService {
    

        public class GetMaterialsResult
        {
            public int Id {get; set;}
            public string Name { get; set; }
        }
        public ApiResult<List<GetMaterialsResult>> GetMaterials()
        {
            var result = new ApiResult<List<GetMaterialsResult>>();
            result.Data = new List<GetMaterialsResult>();
            result.Data = Modules.DbContext.Materials.OrderByDescending(u => u.CreationTime).Select( u => new GetMaterialsResult {
                Id = u.Id,
                Name = u.Name
            }).ToList();
            return result;
        }
        
    }
}