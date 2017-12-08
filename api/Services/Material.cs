using System.Linq;
using api.DataContext;
using api.Models;
using Microsoft.AspNetCore.Http;
using api.Models.Data.Materials;

namespace api.Services
{
    public class MaterialService {
        private ISession Session {get; set;}
        public MaterialService(ISession session)
        {
            this.Session = session;
        }

        public ApiResult<bool> AddMaterial(HesabdarContext context, string name)
        {
            var result = new ApiResult<bool>();

            var data = context.Materials
            .Any(u => u.Name == name);
            if (data)
            {
                result.Data = false;
                result.Messages.Add("کالایی با همین نام در سیستم موجود است");
            } else {
                var userId = new AccountService(Session).GetCurrentUserId();
                var material = new Material {
                    Name = name,
                    CreatorId = userId
                };

                context.Materials.Add(material);

                context.SaveChanges();
                result.Messages.Add("کالا با موفقیت ثبت شد");
            }
            return result;
        }

        public ApiResult<bool> DeleteMaterial(HesabdarContext context, string name)
        {
            var result = new ApiResult<bool>();

            var data = context.Materials
            .Any(u => u.Name == name);
            

            if (!data)
            {
                result.Data = false;
                result.Messages.Add("کالایی این نام در سیستم موجود نیست");
            } else {
                var userId = new AccountService(Session).GetCurrentUserId();
                var material = new Material {
                    Name = name,
                    CreatorId = userId
                };

                context.Materials.Add(material);

                context.SaveChanges();
                result.Messages.Add("کالا با موفقیت حذف شد");
            }
            return result;
        }
        
    }
}