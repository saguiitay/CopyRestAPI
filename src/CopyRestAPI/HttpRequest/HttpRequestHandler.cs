using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CopyRestAPI.Models;

namespace CopyRestAPI.HttpRequest
{
    public class HttpRequestHandler
    {
        private readonly HttpClient _client;
        private readonly HttpClient _uploadsClient;
        
        public HttpRequestHandler()
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            _client = new HttpClient(handler);
            _uploadsClient = new HttpClient(handler) { Timeout = TimeSpan.FromHours(5) };
        }

        public async Task<HttpResponseMessage> ExecuteAsync(HttpRequestItem httpRequestItem, HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
        {
            var httpRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri(httpRequestItem.URL),
                    Method = httpRequestItem.HttpMethod,
                    Content = httpRequestItem.HttpContent
                };
            
            httpRequest.Headers.Add("Authorization", httpRequestItem.AuthzHeader);

            if (httpRequestItem.IsDataRequest)
            {
                httpRequest.Headers.Add("X-Api-Version", "1");
                httpRequest.Headers.Add("Accept", "application/json");
            }

            if (httpRequestItem.Headers != null)
            {
                foreach (var kvp in httpRequestItem.Headers)
                {
                    httpRequest.Headers.Add(kvp.Key, kvp.Value);
                }
            }

            var client = httpRequestItem.IsFileUpload ? _uploadsClient : _client;

            HttpResponseMessage httpResponse = await client.SendAsync(httpRequest, httpCompletionOption).ConfigureAwait(false);

            return httpResponse;
        }
    }
}
