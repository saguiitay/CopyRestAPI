using System;
using System.Threading.Tasks;
using CopyRestAPI.Helpers;
using CopyRestAPI.HttpRequest;
using CopyRestAPI.Models;

namespace CopyRestAPI.Authentication
{
    public class Authorization : IAuthorization
    {
        public Config Config { get; set; }
        
        public Uri AuthCodeUri { get; set; }

        private readonly AuthorizationHeader _authorizationHeader = new AuthorizationHeader();
        private readonly HttpRequestHandler _httpRequestHandler;

        public Authorization(Config config)
        {
            Config = config;

            _httpRequestHandler = new HttpRequestHandler();
        }

        public async Task<OAuthToken> GetRequestTokenAsync()
        {
            string url;
            if (Config.Scope != null)
            {
                string serializedScope = Newtonsoft.Json.JsonConvert.SerializeObject(Config.Scope);

                url = string.Format("{0}?scope={1}", Consts.RequestToken, System.Net.WebUtility.UrlEncode(serializedScope));
            }
            else
            {
                url = Consts.RequestToken;
            }

            string authzHeader = _authorizationHeader.CreateForRequest(Config.CallbackUrl, Config.ConsumerKey, Config.ConsumerSecret, url);

            var httpRequestItem = new HttpRequestItem
            {
                URL = url,
                HttpMethod = System.Net.Http.HttpMethod.Get,
                AuthzHeader = authzHeader,
                HttpContent = null,
                IsDataRequest = false
            };

            var response = await _httpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new ServerException((int)response.StatusCode, result);
            }


            if (string.IsNullOrEmpty(result))
                throw new ServerException((int)response.StatusCode, "Server returned an empty result");

            string[] kvpairs = result.Split('&');
            System.Collections.Generic.Dictionary<string, string> parameters = System.Linq.Enumerable.ToDictionary(System.Linq.Enumerable.Select(kvpairs, pair => pair.Split('=')), kv => kv[0], kv => kv[1]);

            Config.Token = parameters["oauth_token"];
            Config.TokenSecret = parameters["oauth_token_secret"];
            var authCodeUri = new Uri(string.Format("{0}?oauth_token={1}", Consts.Authorize, Config.Token));
            return new OAuthToken
                {
                    Token = Config.Token,
                    TokenSecret = Config.TokenSecret,
                    AuthCodeUri = authCodeUri
                };
        }

        public async Task GetAccessTokenAsync(string verifier)
        {
            string authzHeader = _authorizationHeader.CreateForAccess(Config.ConsumerKey, Config.ConsumerSecret, Config.Token, Config.TokenSecret, verifier);

            var httpRequestItem = new HttpRequestItem
                {
                    URL = Consts.AccessToken,
                    HttpMethod = System.Net.Http.HttpMethod.Get,
                    AuthzHeader = authzHeader,
                    HttpContent = null,
                    IsDataRequest = false
                };

            var response = await _httpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new ServerException((int)response.StatusCode, result);
            }

            string[] kvpairs = result.Split('&');
            System.Collections.Generic.Dictionary<string, string> parameters = System.Linq.Enumerable.ToDictionary(System.Linq.Enumerable.Select(kvpairs, pair => pair.Split('=')), kv => kv[0], kv => kv[1]);

            string oauth_error_message;
            if (parameters.TryGetValue("oauth_error_message", out oauth_error_message))
            {
                throw new ServerException(oauth_error_message);
            }

            Config.Token = parameters["oauth_token"];
            Config.TokenSecret = parameters["oauth_token_secret"];
        }
    }
}
