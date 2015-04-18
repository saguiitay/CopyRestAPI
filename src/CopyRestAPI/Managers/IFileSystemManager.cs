using System.IO;
using System.Threading.Tasks;
using CopyRestAPI.Models;

namespace CopyRestAPI.Managers
{
    public interface IFileSystemManager
    {
        Task<FileSystem> GetFileSystemInformationAsync(string id);
        Task DownloadFileStreamAsync(string fileId, Stream targetStream);
        Task<byte[]> DownloadThumbnailImageAsync(string fileId, int size);
        Task<bool> RenameFileAsync(string fileId, string newFileName, bool overwriteFileWithTheSameName);
        Task<bool> MoveFileAsync(string fileId, string newParentFolderId, string newFileName, bool overwriteFileWithTheSameName);
        Task<FileSystem> CreateNewFolderAsync(string parentFolderId, string folderName, bool overwriteFolderWithTheSameName);
        Task<FileSystem> UploadNewFileStreamAsync(string parentFolderId, string fileName, Stream newFile, bool overwriteFileWithTheSameName);
        Task<bool> DeleteAsync(string id);
        Task<FileSystem> ListFileRevisionsAsync(string fileId);
    }
}