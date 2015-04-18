using Newtonsoft.Json;

namespace CopyRestAPI.Models
{
    public class FileSystemRevision : FileSystemBase
    {
        [JsonProperty(PropertyName = "revision_id", NullValueHandling = NullValueHandling.Ignore)]
        public int RevisionID { get; set; }

        [JsonProperty(PropertyName = "latest", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsLatest { get; set; }

        [JsonProperty(PropertyName = "conflict", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsConflict { get; set; }

        [JsonProperty(PropertyName = "creator", NullValueHandling = NullValueHandling.Ignore)]
        public User Creator { get; set; }
    }
}