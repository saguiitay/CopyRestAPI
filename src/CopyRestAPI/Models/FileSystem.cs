using System;
using CopyRestAPI.Helpers;
using Newtonsoft.Json;

namespace CopyRestAPI.Models
{
    public class FileSystem : FileSystemBase
    {        
        [JsonProperty(PropertyName = "path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public FileSystemType Type { get; set; }

        [JsonProperty(PropertyName = "stub", NullValueHandling = NullValueHandling.Ignore)]
        public bool Stub { get; set; }

        [JsonProperty(PropertyName = "children", NullValueHandling = NullValueHandling.Ignore)]
        public FileSystem[] Children { get; set; }        

        [JsonProperty(PropertyName = "date_last_synced", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime DateLastSynced { get; set; }

        [JsonProperty(PropertyName = "modified_time", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime ModifiedTime { get; set; }

        [JsonProperty(PropertyName = "url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "revision_id", NullValueHandling = NullValueHandling.Ignore)]
        public int RevisionID { get; set; }

        [JsonProperty(PropertyName = "revisions", NullValueHandling = NullValueHandling.Ignore)]
        public FileSystemRevision[] FileSystemRevisions { get; set; }

        [JsonProperty(PropertyName = "token", NullValueHandling = NullValueHandling.Ignore)]
        public string token { get; set; }

        //[JsonProperty(PropertyName = "counts", NullValueHandling = NullValueHandling.Ignore)]
        //public Counts Counts { get; set; }
    }
}
