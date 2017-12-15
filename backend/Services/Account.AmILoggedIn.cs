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
        
        public ApiResult<PermissionLevel> GetPermissionLevel()
        {
            var result = new ApiResult<PermissionLevel>();

            result.Data = PermissionLevel.Administrator;

            return result;
        }
    }
}