using Newtonsoft.Json;

namespace CopyRestAPI.Models.Permissions
{
    public class ProfilePermission : WritePermisson
    {
        [JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
        public EmailPermission Email { get; set; }
    }
}