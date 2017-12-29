using System;
using System.Linq;
using api.DataContext;
using api.Models;
using api.Models.Data.Account;
using api.Providers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Services {
    public partial class AccountService : BaseService {
        
        public ApiResult Login(string username, string password)
        {
            var result = new ApiResult();
            
            var data = this.Modules.DbContext.ActiveUsers
            .Where(u => u.Username == username && u.Password == password)
            .Select(u => u.Id)
            .FirstOrDefault();
            if (data != 0)
            {
                this.Modules.Session.SetInt32("UserId", data);
                result.Messages.Add("با موفقیت وارد شدید");
            } else {
                result.ResultStatus = ResultStatus.Failed;
                result.Messages.Add("کاربری با مشخصات وارد شده پیدا نشد");
                
            }

            return result;
        }
    }
}