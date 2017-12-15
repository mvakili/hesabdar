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
        
        public class ProfileResult
        {
            public string Username { get; set; }
            public string Firstname { get; set; }
            public string Lastname {get; set;}
            public bool Deleted {get; set;}
        }
        public ApiResult<ProfileResult> GetProfile(string username)
        {
            var result = new ApiResult<ProfileResult>();

            result.Data = Modules.DbContext.Users
            .Where(u => u.Username == username)
            .Select(u => new ProfileResult {
                Username = u.Username,
                Firstname = "",
                Lastname = "",
                Deleted = u.Deleted
            })
            .FirstOrDefault();

            return result;
            
        }
    }
}