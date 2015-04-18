using CopyRestAPI.Models.Permissions;
using Newtonsoft.Json;

namespace CopyRestAPI.Models
{
    public class Scope
    {
        [JsonProperty(PropertyName = "profile", NullValueHandling = NullValueHandling.Ignore)]
        public ProfilePermission Profile { get; set; }

        [JsonProperty(PropertyName = "inbox", NullValueHandling = NullValueHandling.Ignore)]
        public InboxPermission Inbox { get; set; }

        [JsonProperty(PropertyName = "links", NullValueHandling = NullValueHandling.Ignore)]
        public LinksPermission Links { get; set; }

        [JsonProperty(PropertyName = "filesystem", NullValueHandling = NullValueHandling.Ignore)]
        public FileSystemPermission FileSystem { get; set; }
    }
}
