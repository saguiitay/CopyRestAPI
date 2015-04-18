using System.Threading.Tasks;
using CopyRestAPI.Authentication;
using CopyRestAPI.Managers;
using CopyRestAPI.Models;

namespace CopyRestAPI
{
    public interface ICopyClient
    {
        IAuthorization Authorization { get; }
        IUserManager UserManager { get; }
        IFileSystemManager FileSystemManager { get; }
        ILinkManager LinkManager { get; }

        Task<FileSystem> GetRootFolder();
        Task<FileSystem> GetSharedFolder();
    }
}