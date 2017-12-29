using System;
using api.Models;
using api.Controllers;
using api.DataContext;
using Microsoft.AspNetCore.Http;
using api.Services;
using api.Models.Data.Account;

namespace api.Providers
{
    public class Job<T> : BaseJob
    {
        public delegate void Work(ref ApiResult<T> result);
        public  ApiResult<T> Run(Work Do)
        {
            var result = new ApiResult<T> { ResultStatus = ResultStatus.Successful };
            try
            {
                if (Permission != PermissionLevel.General)
                {
                    var res = new Services.AccountService(Controller.ModuleContainer).GetPermissionLevel();
                    result.ResultStatus = res.ResultStatus;
                    result.Messages = res.Messages;
                    if (!BaseJob.HasAccess(res.Data, Permission))
                    {
                        result.ResultStatus = ResultStatus.Unauthorized;
                        return result;
                    }
                }
                Do(ref result);
            }
            catch (Exception ex)
            {
                result.Messages.Clear();
                result.ResultStatus = ResultStatus.Thrown;
            }
            return result;
        }
    }


    public class Job : BaseJob
    {
        public delegate void Work(ref ApiResult result);
        public ApiResult Run(Work Do)
        {
            var result = new ApiResult { ResultStatus = ResultStatus.Successful };
            try
            {
                if (Permission != PermissionLevel.General)
                {
                    var res = new Services.AccountService(this.Controller.ModuleContainer).GetPermissionLevel();
                    result.ResultStatus = res.ResultStatus;
                    result.Messages = res.Messages;
                    if (!BaseJob.HasAccess(res.Data, Permission))
                    {
                        result.ResultStatus = ResultStatus.Unauthorized;
                        return result;
                    }
                }
                Do(ref result);
            }
            catch (Exception)
            {
                result.Messages.Clear();
                result.ResultStatus = ResultStatus.Thrown;
            }
            return result;
        }
    }
    public class DataJob<T> : BaseJob
    {
        public delegate void Work(HesabdarContext context,ref ApiResult<T> result);
        public ApiResult<T> Run(Work Do)
        {
            var result = new ApiResult<T> { ResultStatus = ResultStatus.Successful };
            try
            {
                if (Permission != PermissionLevel.General)
                {
                    var res = new Services.AccountService(this.Controller.ModuleContainer).GetPermissionLevel();                    
                    result.ResultStatus = res.ResultStatus;
                    result.Messages = res.Messages;
                    if (!BaseJob.HasAccess(res.Data, Permission))
                    {
                        result.ResultStatus = ResultStatus.Unauthorized;
                        return result;
                    }
                }

                using (var transaction = Controller.ModuleContainer.DbContext.Database.BeginTransaction())
                {
                    Do(Controller.ModuleContainer.DbContext,ref result);
                    transaction.Commit();
                }

            }
            catch (Exception)
            {
                result.Messages.Clear();
                result.ResultStatus = ResultStatus.Thrown;
            }

            return result;
        }
    }

    public class DataJob : BaseJob
    {
        public delegate void Work(HesabdarContext context,ref ApiResult result);
        public ApiResult Run(Work Do)
        {
            var result = new ApiResult { ResultStatus = ResultStatus.Successful };
            try
            {
                if (Permission != PermissionLevel.General)
                {
                    var res = new Services.AccountService(Controller.ModuleContainer).GetPermissionLevel();                    
                    result.ResultStatus = res.ResultStatus;
                    result.Messages = res.Messages;
                    if (!BaseJob.HasAccess(res.Data, Permission))
                    {
                        result.ResultStatus = ResultStatus.Unauthorized;
                        return result;
                    }
                }

                using (var transaction = Controller.ModuleContainer.DbContext.Database.BeginTransaction())
                {
                    Do(Controller.ModuleContainer.DbContext, ref result);
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                result.Messages.Clear();
                result.ResultStatus = ResultStatus.Thrown;
            }

            return result;
        }
    }

    public class BaseJob
    {
        public PermissionLevel Permission { get; set; } = PermissionLevel.General;
        public HesabdarController Controller {get; set;}

        protected static bool HasAccess(PermissionLevel ToCheckPermissionLevel, PermissionLevel BasePermissionLevel)
        {

            switch (BasePermissionLevel)
            {
                case PermissionLevel.General:
                    return true;
                case PermissionLevel.Spectrator:
                    return 
                    (ToCheckPermissionLevel == PermissionLevel.Spectrator) ||
                    (ToCheckPermissionLevel == PermissionLevel.User) ||
                    (ToCheckPermissionLevel == PermissionLevel.Administrator);
                case PermissionLevel.User:
                    return
                    (ToCheckPermissionLevel == PermissionLevel.User) ||
                    (ToCheckPermissionLevel == PermissionLevel.Administrator);
                case PermissionLevel.Administrator:
                    return
                    (ToCheckPermissionLevel == PermissionLevel.Administrator);
                default:
                    return false;
            }
        }

        public T UseService<T> () where T:BaseService {

            var result = (T)Activator.CreateInstance(typeof(T));

            result.Modules = Controller.ModuleContainer;

            return result;
        }
    }
}
