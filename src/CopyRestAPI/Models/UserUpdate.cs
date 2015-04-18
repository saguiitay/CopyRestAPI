using Newtonsoft.Json;

namespace CopyRestAPI.Models
{
    public class UserUpdate
    {
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }
    }
}