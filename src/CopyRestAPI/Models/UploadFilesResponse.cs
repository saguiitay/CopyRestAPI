using Newtonsoft.Json;

namespace CopyRestAPI.Models
{
    internal class UploadFilesResponse
    {
        [JsonProperty(PropertyName = "objects", NullValueHandling = NullValueHandling.Ignore)]
        public FileSystem[] Objects { get; set; }
    }
}