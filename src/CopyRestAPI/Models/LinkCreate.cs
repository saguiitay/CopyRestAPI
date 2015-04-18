using Newtonsoft.Json;

namespace CopyRestAPI.Models
{
    public class LinkCreate
    {
        [JsonProperty(PropertyName = "public", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsPublic { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "paths", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Paths { get; set; }
    }
}