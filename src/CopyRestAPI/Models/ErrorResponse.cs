using Newtonsoft.Json;

namespace CopyRestAPI.Models
{
    public class ErrorResponse
    {
        [JsonProperty(PropertyName = "error", NullValueHandling = NullValueHandling.Ignore)]
        public int Error { get; set; }

        [JsonProperty(PropertyName = "message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}