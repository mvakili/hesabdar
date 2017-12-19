using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace api.Models
{
    public class ApiResult
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ResultStatus ResultStatus { get; set; } = ResultStatus.Successful;
        public List<string> Messages { get; set; } = new List<string>();
    }

    public class ApiResult<T> : ApiResult
    {
        public T Data { get; set; }
    }
}