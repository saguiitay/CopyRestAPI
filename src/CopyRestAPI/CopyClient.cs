using System.Threading.Tasks;
using CopyRestAPI.Authentication;
using CopyRestAPI.Helpers;
using CopyRestAPI.Managers;
using CopyRestAPI.Models;

namespace CopyRestAPI
{
    public class CopyClient : ICopyClient
    {
        private readonly AuthorizationHeader _authorizationHeader;
        public Config Config { get; set; }

        public IAuthorization Authorization { get; set; }
        public IUserManager UserManager { get; set; }
        public IFileSystemManager FileSystemManager { get; set; }
        public ILinkManager LinkManager { get; set; }

        public CopyClient(Config config)
        {
            _authorizationHeader = new AuthorizationHeader();
            Config = config;


            InitManagers();
        }

        private void InitManagers()
        {
            Authorization = new Authorization(Config);
            UserManager = new UserManager(Config, _authorizationHeader);
            FileSystemManager = new FileSystemManager(Config, _authorizationHeader);
            LinkManager = new LinkManager(Config, _authorizationHeader);
        }

        public Task<FileSystem> GetRootFolder()
        {
            return FileSystemManager.GetFileSystemInformationAsync("/copy");
        }

        public Task<FileSystem> GetSharedFolder()
        {
            return FileSystemManager.GetFileSystemInformationAsync("/inbox");
        } 
    }
}
