using System.Threading.Tasks;
using CopyRestAPI.Models;

namespace CopyRestAPI.Authentication
{
    public interface IAuthorization
    {        
        Task<OAuthToken> GetRequestTokenAsync();
        Task GetAccessTokenAsync(string verifier);
    }
}
