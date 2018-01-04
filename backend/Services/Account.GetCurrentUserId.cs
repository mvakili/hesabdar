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
        
        public int? GetCurrentUserId()
        {
            try
            {
                return 1;
                // return 2;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}