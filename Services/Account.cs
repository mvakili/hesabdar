using System;
using System.Linq;
using api.DataContext;
using api.Models;
using api.Models.Data.Account;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Services {
    public class AccountService {
        private ISession Session {get; set;}
        public AccountService(ISession session)
        {
            this.Session = session;
        }
        public ApiResult<bool> AmILoggedIn()
        {
            var result = new ApiResult<bool>();

            result.Data = true;

            return result;
        }

        public ApiResult<bool> AddUser(HesabdarContext context, string username, string password)
        {
            var result = new ApiResult<bool>();


            var data = context.Users
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

                context.Users.Add(user);
                context.SaveChanges();
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
                Session.SetInt32("UserId", data);
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

            Session.Remove("UserId");

            return result;
        }

        public int? GetCurrentUserId()
        {
            try
            {
                return Int32.Parse(Session.GetString("UserId"));
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}