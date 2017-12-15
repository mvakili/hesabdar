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
        
        public ApiResult<bool> AddUser(string username, string password)
        {
            var result = new ApiResult<bool>();


            var data = this.Modules.DbContext.Users
            .Any(u => u.Username == username);

            if (data)
            {
                result.Data = false;
                result.Messages.Add("کاربری با این نام کاربری در سیستم موجود است");
            } else {
                var currentUserId = this.GetCurrentUserId();
                var user = new User
                {
                    Username = username,
                    Password = password,
                    CreatorId = currentUserId
                };

                this.Modules.DbContext.Users.Add(user);
                this.Modules.DbContext.SaveChanges();
                result.Data = true;
                result.Messages.Add("کاربر با موفقیت ایجاد شد");
                
            }
            
            return result;
        }
    }
}