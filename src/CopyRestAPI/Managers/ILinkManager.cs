using System.Threading.Tasks;
using CopyRestAPI.Models;

namespace CopyRestAPI.Managers
{
    public interface ILinkManager
    {
        Task<Link> GetLinkInformationAsync(string token);
        Task<Link[]> GetAllLinksAsync();
        Task<Link> CreateLink(LinkCreate newLink);
    }
}