using System;
using api.Models;
using api.Controllers;
using api.DataContext;
using Microsoft.AspNetCore.Http;
using api.Services;

namespace api.Providers
{
    public class Job<T> : BaseJob
    {
        public delegate void Work(ApiResult<T> result);
        public  ApiResult<T> Run(Work Do)
        {
            var result = new ApiResult<T> { ResultStatus = ResultStatus.Successful };
            try
            {
                if (Authorized)
                {
                    var res = new Services.AccountService(Controller.ModuleContainer).AmILoggedIn();
                    result.ResultStatus = res.ResultStatus;
                    result.Messages = res.Messages;
                    if (res.ResultStatus == ResultStatus.Successful && !res.Data)
                    {
                        result.ResultStatus = ResultStatus.Unauthorized;
                        return result;
                    }
                }
                Do(result);
            }
            catch (Exception)
            {
                result.Messages.Clear();
                result.ResultStatus = ResultStatus.Thrown;
            }
            return result;
        }
    }


    public class Job : BaseJob
    {
        public delegate void Work(ApiResult result);
        public ApiResult Run(Work Do)
        {
            var result = new ApiResult { ResultStatus = ResultStatus.Successful };
            try
            {
                if (Authorized)
                {
                    var res = new Services.AccountService(this.Controller.ModuleContainer).AmILoggedIn();
                    result.ResultStatus = res.ResultStatus;
                    result.Messages = res.Messages;
                    if (res.ResultStatus == ResultStatus.Successful && !res.Data)
                    {
                        result.ResultStatus = ResultStatus.Unauthorized;
                        return result;
                    }
                }
                Do(result);
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
        public delegate void Work(HesabdarContext context, ApiResult<T> result);
        public ApiResult<T> Run(Work Do)
        {
            var result = new ApiResult<T> { ResultStatus = ResultStatus.Successful };
            try
            {
                if (Authorized)
                {
                    var res = new Services.AccountService(this.Controller.ModuleContainer).AmILoggedIn();                    
                    result.ResultStatus = res.ResultStatus;
                    result.Messages = res.Messages;
                    if (res.ResultStatus == ResultStatus.Successful && !res.Data)
                    {
                        result.ResultStatus = ResultStatus.Unauthorized;
                        return result;
                    }
                }

                using (var transaction = Controller.ModuleContainer.DbContext.Database.BeginTransaction())
                {
                    Do(Controller.ModuleContainer.DbContext, result);
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
        public delegate void Work(HesabdarContext context, ApiResult result);
        public ApiResult Run(Work Do)
        {
            var result = new ApiResult { ResultStatus = ResultStatus.Successful };
            try
            {
                if (Authorized)
                {
                    var res = new Services.AccountService(Controller.ModuleContainer).AmILoggedIn();                    
                    result.ResultStatus = res.ResultStatus;
                    result.Messages = res.Messages;
                    if (res.ResultStatus == ResultStatus.Successful && !res.Data)
                    {
                        result.ResultStatus = ResultStatus.Unauthorized;
                        return result;
                    }
                }

                using (var transaction = Controller.ModuleContainer.DbContext.Database.BeginTransaction())
                {
                    Do(Controller.ModuleContainer.DbContext, result);
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
        public bool Authorized { get; set; } = true;
        public HesabdarController Controller {get; set;}
        public T UseService<T> () where T:BaseService {

            var result = (T)Activator.CreateInstance(typeof(T));

            result.Modules = Controller.ModuleContainer;

            return result;
        }
    }
}
