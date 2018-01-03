using System.Linq;
using api.DataContext;
using api.Models;
using Microsoft.AspNetCore.Http;
using api.Models.Data.Materials;
using api.Providers;

namespace api.Services
{
    public partial class MaterialService : BaseService {
    
        public ApiResult<int> AddMaterial(string name)
        {
            var result = new ApiResult<int>();

            var data = this.Modules.DbContext.Materials
            .Where(u => u.Name == name).Select(u => u.Id).FirstOrDefault();
            if (data != 0)
            {
                result.ResultStatus = ResultStatus.Failed;
                result.Data = data;
                result.Messages.Add("کالایی با همین نام در سیستم موجود است");
            } else {
                var userId = new AccountService(Modules).GetCurrentUserId();
                var material = new Material {
                    Name = name,
                    CreatorId = userId
                };

                this.Modules.DbContext.Materials.Add(material);

                this.Modules.DbContext.SaveChanges();
                result.Data = material.Id;
                result.Messages.Add("کالا با موفقیت ثبت شد");
            }
            return result;
        }
        
    }
}