using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CopyRestAPI.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LinkPermissionType
    {
        Read,
        Sync
    }
}