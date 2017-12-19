using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace api.Models.Data.Account {
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PermissionLevel {
        General,
        Spectrator,
        User,
        Administrator
    }
}