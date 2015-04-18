using System.Threading.Tasks;
using CopyRestAPI.Models;

namespace CopyRestAPI.Managers
{
    public interface IUserManager
    {
        Task<User> GetUserAsync();
        Task<User> UpdateUserAsync(UserUpdate userUpdate);
    }
}