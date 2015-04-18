using Newtonsoft.Json;

namespace CopyRestAPI.Models
{
    public class FileSystemBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "size", NullValueHandling = NullValueHandling.Ignore)]
        public long Size { get; set; }
    }
}