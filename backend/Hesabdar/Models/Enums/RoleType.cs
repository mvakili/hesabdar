using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hesabdar.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RoleType : byte
    {
        Spectrator,
        Admin,
        Employee
    }
}