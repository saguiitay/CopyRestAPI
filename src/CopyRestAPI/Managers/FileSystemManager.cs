using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CopyRestAPI.Helpers;
using CopyRestAPI.Models;
using Newtonsoft.Json;

namespace CopyRestAPI.Managers
{
    public class FileSystemManager : BaseManager, IFileSystemManager
    {
        public FileSystemManager(Config config, AuthorizationHeader authorizationHeader)
            : base(config, authorizationHeader)
        {
        }

        public async Task<FileSystem> GetFileSystemInformationAsync(string id)
        {
            id = NormalizeId(id);

            if (id == null)
                throw new ArgumentNullException("id");

            string url = string.Format("{0}{1}", Consts.Meta, id);

            var httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Get);

            using (var response = await HttpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false))
            {
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ServerException((int) response.StatusCode, result);
                }

                return JsonConvert.DeserializeObject<FileSystem>(result);
            }
        }

        public async Task DownloadFileStreamAsync(string fileId, Stream targetStream)
        {
            fileId = NormalizeId(fileId);

            if (fileId == null)
                throw new ArgumentNullException("fileId");
            if (targetStream == null)
                throw new ArgumentNullException("targetStream");

            fileId = fileId.Replace("/copy", "/files");

            string url = string.Format("{0}{1}", Consts.RestRoot, fileId);

            var httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Get);

            long? length = null;
            string etag = null;


            const long bufferSize = 4*1024*1024;
            long? read = 0;
            HttpResponseMessage restResponse;
            do
            {
                var from = read;
                long? to = read + bufferSize;
                if (length != null && to > length)
                    to = length;

                httpRequestItem.AddHeader("Range", string.Format("bytes={0}-{1}", from, to));

                restResponse = await HttpRequestHandler.ExecuteAsync(httpRequestItem, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                await restResponse.Content.CopyToAsync(targetStream).ConfigureAwait(false);


                read += restResponse.Content.Headers.ContentLength;
                if (length == null && restResponse.Content != null && restResponse.Content.Headers != null && restResponse.Content.Headers.ContentRange != null)
                    length = restResponse.Content.Headers.ContentRange.Length;

                IEnumerable<string> etags;
                if (etag == null && restResponse.Headers.TryGetValues("etag", out etags))
                    etag = etags.FirstOrDefault();

                if (read >= length)
                    break;
            } while (restResponse.StatusCode == HttpStatusCode.PartialContent && restResponse.Content.Headers.ContentLength > 0);
        }

        public async Task<byte[]> DownloadThumbnailImageAsync(string fileId, int size)
        {
            fileId = NormalizeId(fileId);

            if (fileId == null)
                throw new ArgumentNullException("fileId");

            fileId = fileId.Replace("/copy", "/thumbs");

            string url = string.Format("{0}{1}", Consts.RestRoot, fileId);

            var httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Get);

            using (var response = await HttpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false))
            {

                if (!response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new ServerException((int) response.StatusCode, result);
                }

                return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            }
        }

        public async Task<bool> RenameFileAsync(string fileId, string newFileName, bool overwriteFileWithTheSameName)
        {
            bool result = false;

            fileId = NormalizeId(fileId);

            if (fileId == null)
                throw new ArgumentNullException("fileId");

            fileId = fileId.Replace("/copy", "/files");

            string url = string.Format("{0}{1}?name={2}&overwrite={3}", Consts.RestRoot, fileId, newFileName,
                overwriteFileWithTheSameName.ToLowerString());

            var httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Put);

            using (var httpResponseMessage = await HttpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false))
            {

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    result = true;
                }

                return result;
            }
        }

        public async Task<bool> MoveFileAsync(string fileId, string newParentFolderId, string newFileName, bool overwriteFileWithTheSameName)
        {
            bool result = false;

            fileId = NormalizeId(fileId);
            string targetFileId = NormalizeId(string.Format("{0}/{1}", newParentFolderId, newFileName));

            if (fileId == null)
                throw new ArgumentNullException("fileId");
            if (string.IsNullOrEmpty(newParentFolderId))
                throw new ArgumentException("newParentFolderId");
            if (string.IsNullOrEmpty(newFileName))
                throw new ArgumentException("newFileName");

            fileId = fileId.Replace("/copy", "/files");

            string url = string.Format("{0}{1}?path={2}&overwrite={3}", Consts.RestRoot, fileId, targetFileId,
                overwriteFileWithTheSameName.ToLowerString());

            var httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Put);

            using (var httpResponseMessage = await HttpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false))
            {

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    result = true;
                }

                return result;
            }
        }

        public async Task<FileSystem> CreateNewFolderAsync(string parentFolderId, string folderName, bool overwriteFolderWithTheSameName)
        {
            parentFolderId = NormalizeId(parentFolderId);

            if (parentFolderId == null)
                throw new ArgumentNullException("parentFolderId");

            parentFolderId = parentFolderId.Replace("/copy", "/files");

            string url = string.Format("{0}{1}/{2}?overwrite={3}", Consts.RestRoot, parentFolderId, folderName,
                overwriteFolderWithTheSameName.ToLowerString());

            var httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Post);

            using (var response = await HttpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false))
            {
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ServerException((int) response.StatusCode, result);
                }

                return JsonConvert.DeserializeObject<FileSystem>(result);
            }
        }

        public async Task<FileSystem> UploadNewFileStreamAsync(string parentFolderId, string fileName, Stream newFile, bool overwriteFileWithTheSameName)
        {
            parentFolderId = NormalizeId(parentFolderId);

            if (parentFolderId == null)
                throw new ArgumentNullException("parentFolderId");
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("fileName");
            if (newFile.Length > 1073741824)
                throw new ArgumentException("Cannot upload files larger than 1GB", "newFile");

            parentFolderId = parentFolderId.Replace("/copy", "/files");

            string url = string.Format("{0}{1}?overwrite={2}", Consts.RestRoot, parentFolderId,
                overwriteFileWithTheSameName.ToLowerString());

            using (HttpContent httpContent = new StreamContent(newFile))
            {

                var contentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "file",
                        FileName = fileName
                    };

                httpContent.Headers.ContentDisposition = contentDisposition;

                using (var formContent = new MultipartFormDataContent(new Random().Next(10000, 99999).ToString())
                    {
                        httpContent
                    })
                {


                    var httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Post, formContent);
                    httpRequestItem.IsFileUpload = true;

                    using (var response = await HttpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false))
                    {
                        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new ServerException((int) response.StatusCode, result);
                        }

                        var uploadFilesResponse = JsonConvert.DeserializeObject<UploadFilesResponse>(result);
                        return uploadFilesResponse.Objects[0];
                    }
                }
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            bool result = false;

            id = NormalizeId(id);

            if (id == null)
                throw new ArgumentNullException("id");

            id = id.Replace("/copy", "/files");

            string url = string.Format("{0}{1}", Consts.RestRoot, id);

            var httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Delete);

            using (var httpResponseMessage = await HttpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false))
            {
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    result = true;
                }

                return result;
            }
        }

        public async Task<FileSystem> ListFileRevisionsAsync(string fileId)
        {
            fileId = NormalizeId(fileId);

            if (fileId == null)
                throw new ArgumentNullException("fileId");

            string url = string.Format("{0}/meta{1}/@activity", Consts.RestRoot, fileId);

            var httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Get);

            using (var response = await HttpRequestHandler.ExecuteAsync(httpRequestItem).ConfigureAwait(false))
            {
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ServerException((int) response.StatusCode, result);
                }

                return JsonConvert.DeserializeObject<FileSystem>(result);
            }
        }

        private string NormalizeId(string id)
        {
            if (id != null)
            {
                if (!id.StartsWith("/"))
                {
                    id = "/" + id;
                }
            }

            return id;
        }
    }
}