using Newtonsoft.Json;

namespace CopyRestAPI.Models.Permissions
{
    public class WritePermisson : ReadPermisson
    {
        [JsonProperty(PropertyName = "write")]
        public bool Write { get; set; }
    }
}