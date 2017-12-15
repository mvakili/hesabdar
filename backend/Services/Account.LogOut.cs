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
        
        public ApiResult LogOut()
        {
            var result = new ApiResult();

            this.Modules.Session.Remove("UserId");

            return result;
        }
    }
}