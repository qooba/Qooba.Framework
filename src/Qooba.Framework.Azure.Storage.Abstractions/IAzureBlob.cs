using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Qooba.Framework.Azure.Storage.Abstractions
{
    public interface IAzureBlob
    {
        Task<string> GenerateBlobUri(string containerName, string blobName, DateTime startDate, DateTime endDate, params Models.SharedAccessBlobPermissions[] permissions);

        Task CreateBlobContainer(string containerName);

        Task UploadBlob(string containerName, string blobName, string filePath);

        Task UploadFromStreamAsync(string containerName, string blobName, Stream stream);

        Task DownloadToStreamAsync(string containerName, string blobName, Stream target);

        Task DownloadBlob(string containerName, string blobName, string filePath);

        Task<string> DownloadBlob(string containerName, string blobName);

        Task DeleteBlob(string containerName, string blobName);

        Task AppendBlob(string containerName, string blobName, string content);

        Task<IList<string>> ListBlobs(string containerName);
    }
}
