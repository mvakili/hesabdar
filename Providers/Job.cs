using System;
using api.Models;
using api.Controllers;
using api.DataContext;

namespace api.Providers
{
    public class Job<T>
    {
        public delegate void Work(ApiResult<T> result);
        public Work Do { get; set; }
        public bool Authorized { get; set; } = true;
        public  ApiResult<T> Run()
        {
            var result = new ApiResult<T> { ResultStatus = ResultStatus.Successful };
            try
            {
                if (Authorized)
                {
                    var res = new AccountController().AmILoggedIn();
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

    public class Job
    {
        public delegate void Work(ApiResult result);
        public Work Do { get; set; }
        public bool Authorized { get; set; } = true;
        public ApiResult Run()
        {
            var result = new ApiResult { ResultStatus = ResultStatus.Successful };
            try
            {
                if (Authorized)
                {
                    var res = new AccountController().AmILoggedIn();
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
    public class DataJob<T>
    {
        public delegate void Work(HesabdarContext context, ApiResult<T> result);
        public Work Do { get; set; }
        public bool Authorized { get; set; } = true;
        public ApiResult<T> Run()
        {
            var result = new ApiResult<T> { ResultStatus = ResultStatus.Successful };
            try
            {
                if (Authorized)
                {
                    var res = new AccountController().AmILoggedIn();
                    result.ResultStatus = res.ResultStatus;
                    result.Messages = res.Messages;
                    if (res.ResultStatus == ResultStatus.Successful && !res.Data)
                    {
                        result.ResultStatus = ResultStatus.Unauthorized;
                        return result;
                    }
                }

                using(var context = new HesabdarContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        Do(context, result);
                        transaction.Commit();
                    }
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

    public class DataJob
    {
        public delegate void Work(HesabdarContext context, ApiResult result);

        public Work Do { get; set; }
        public bool Authorized { get; set; } = true;
        public ApiResult Run()
        {
            var result = new ApiResult { ResultStatus = ResultStatus.Successful };
            try
            {
                if (Authorized)
                {
                    var res = new AccountController().AmILoggedIn();
                    result.ResultStatus = res.ResultStatus;
                    result.Messages = res.Messages;
                    if (res.ResultStatus == ResultStatus.Successful && !res.Data)
                    {
                        result.ResultStatus = ResultStatus.Unauthorized;
                        return result;
                    }
                }

                using(var context = new HesabdarContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        Do(context, result);
                        transaction.Commit();
                    }
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


}
