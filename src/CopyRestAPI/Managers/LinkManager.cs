using System;
using System.Net.Http;
using System.Threading.Tasks;
using CopyRestAPI.Helpers;
using CopyRestAPI.Models;
using Newtonsoft.Json;

namespace CopyRestAPI.Managers
{
    public class LinkManager : BaseManager, ILinkManager
    {
        public LinkManager(Config config, AuthorizationHeader authorizationHeader) 
            : base(config, authorizationHeader)
        {
        }

        public async Task<Link> GetLinkInformationAsync(string token)
        {
            if (token == null)
                throw new ArgumentNullException("token");

            string url = string.Format("{0}/{1}", Consts.Links, token);

            var httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Get);

            using (var response = await HttpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false))
            {
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ServerException((int) response.StatusCode, result);
                }

                return JsonConvert.DeserializeObject<Link>(result);
            }
        }

        public async Task<Link[]> GetAllLinksAsync()
        {
            var httpRequestItem = CreateHttpRequestItem(Consts.Links, HttpMethod.Get);

            using (var response = await HttpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false))
            {
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ServerException((int) response.StatusCode, result);
                }

                return JsonConvert.DeserializeObject<Link[]>(result);
            }
        }

        public async Task<Link> CreateLink(LinkCreate newLink)
        {
            if (newLink == null)
                throw new ArgumentNullException("newLink");

            string serializeObject = JsonConvert.SerializeObject(newLink);

            using (var httpContent = new StringContent(serializeObject))
            {
                var httpRequestItem = CreateHttpRequestItem(Consts.Links, HttpMethod.Post, httpContent);

                using (var response = await HttpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false))
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ServerException((int) response.StatusCode, result);
                    }

                    return JsonConvert.DeserializeObject<Link>(result);
                }
            }
        }
    }
}