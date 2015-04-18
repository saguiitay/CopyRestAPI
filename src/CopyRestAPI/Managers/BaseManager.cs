using System.Net.Http;
using CopyRestAPI.Helpers;
using CopyRestAPI.HttpRequest;
using CopyRestAPI.Models;

namespace CopyRestAPI.Managers
{
    public abstract class BaseManager
    {
        protected readonly HttpRequestHandler HttpRequestHandler;
        protected readonly AuthorizationHeader AuthorizationHeader;
        protected Config Config;

        protected BaseManager(Config config, AuthorizationHeader authorizationHeader)
        {
            AuthorizationHeader = authorizationHeader;
            Config = config;

            HttpRequestHandler = new HttpRequestHandler();
        }

        protected HttpRequestItem CreateHttpRequestItem(string url, HttpMethod httpMethod, HttpContent httpContent = null)
        {
            string authzHeader = AuthorizationHeader.CreateForREST(Config.ConsumerKey, Config.ConsumerSecret,
                Config.Token, Config.TokenSecret, url,
                httpMethod.ToString());

            return new HttpRequestItem
                {
                    URL = url,
                    HttpMethod = httpMethod,
                    AuthzHeader = authzHeader,
                    HttpContent = httpContent,
                    IsDataRequest = true
                };
        }


    }
}