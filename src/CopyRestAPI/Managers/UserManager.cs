using System;
using System.Net.Http;
using System.Threading.Tasks;
using CopyRestAPI.Helpers;
using CopyRestAPI.Models;
using Newtonsoft.Json;

namespace CopyRestAPI.Managers
{
    public class UserManager : BaseManager, IUserManager
    {
        public UserManager(Config config, AuthorizationHeader authorizationHeader)
            : base(config, authorizationHeader)
        {
        }

        public async Task<User> GetUserAsync()
        {
            var httpRequestItem = CreateHttpRequestItem(Consts.User, HttpMethod.Get);

            using (var response = await HttpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false))
            {
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ServerException((int) response.StatusCode, result);
                }

                return JsonConvert.DeserializeObject<User>(result);
            }
        }

        public async Task<User> UpdateUserAsync(UserUpdate userUpdate)
        {
            if (userUpdate == null)
                throw new ArgumentNullException("userUpdate");

            string data = JsonConvert.SerializeObject(userUpdate);
            using (var httpContent = new StringContent(data))
            {
                var httpRequestItem = CreateHttpRequestItem(Consts.User, HttpMethod.Put, httpContent);

                using (var response = await HttpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false))
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ServerException((int) response.StatusCode, result);
                    }

                    return JsonConvert.DeserializeObject<User>(result);
                }
            }
        }
    }
}