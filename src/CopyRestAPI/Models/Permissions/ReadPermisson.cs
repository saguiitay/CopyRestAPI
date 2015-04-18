using Newtonsoft.Json;

namespace CopyRestAPI.Models.Permissions
{
    public class ReadPermisson
    {
        [JsonProperty(PropertyName = "read")]
        public bool Read { get; set; }
    }
}