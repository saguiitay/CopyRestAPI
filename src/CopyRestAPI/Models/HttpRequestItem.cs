using System.Collections.Generic;
using System.Net.Http;

namespace CopyRestAPI.Models
{
    public class HttpRequestItem
    {
        public string URL { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public string AuthzHeader { get; set; }

        public HttpContent HttpContent { get; set; }

        public bool IsDataRequest { get; set; }

        public bool IsFileUpload { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public void AddHeader(string key, string value)
        {
            if (Headers == null)
                Headers = new Dictionary<string, string>();

            Headers[key] = value;
        }
    }
}
