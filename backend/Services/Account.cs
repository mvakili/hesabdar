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
    public class AccountService : BaseService {
        public AccountService(ModuleContainer container) : base(container) {}
        public ApiResult<bool> AmILoggedIn()
        {
            var result = new ApiResult<bool>();

            result.Data = true;

            return result;
        }

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

        public ApiResult<bool> Login(HesabdarContext context, string username, string password)
        {
            var result = new ApiResult<bool>();
            
            var data = context.ActiveUsers
            .Where(u => u.Username == username && u.Password == password)
            .Select(u => u.Id)
            .FirstOrDefault();
            if (data != 0)
            {
                this.Modules.Session.SetInt32("UserId", data);
                result.Data = true;
                result.Messages.Add("با موفقیت وارد شدید");
            } else {
                result.Data = false;
                result.Messages.Add("کاربری با مشخصات وارد شده پیدا نشد");
                
            }

            return result;
        }
        
        public ApiResult LogOut()
        {
            var result = new ApiResult();

            this.Modules.Session.Remove("UserId");

            return result;
        }

        public int? GetCurrentUserId()
        {
            try
            {
                return Int32.Parse(this.Modules.Session.GetString("UserId"));
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}