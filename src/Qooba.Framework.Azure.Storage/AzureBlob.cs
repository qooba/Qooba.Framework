using System.Threading.Tasks;
using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Qooba.Framework.Azure.Storage.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Qooba.Framework.Azure.Storage
{
    public class AzureBlob : IAzureBlob
    {
        private readonly IAzureStorageConfig config;

        public AzureBlob(IAzureStorageConfig config)
        {
            this.config = config;
        }

        public async Task<string> GenerateBlobUri(string containerName, string blobName, DateTime startDate, DateTime endDate, params Abstractions.Models.SharedAccessBlobPermissions[] permissions)
        {
            var storageAccount = CloudStorageAccount.Parse(this.config.StorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerName);
            await blobContainer.CreateIfNotExistsAsync();

            var blob = blobContainer.GetBlockBlobReference(blobName);

            SharedAccessBlobPermissions per = SharedAccessBlobPermissions.None;
            foreach (var permission in permissions)
            {
                SharedAccessBlobPermissions p;
                if (Enum.TryParse(permission.ToString(), out p))
                {
                    per |= p;
                }
            }

            var sasConstraints = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = startDate,
                SharedAccessExpiryTime = endDate,
                Permissions = per
            };

            var token = blob.GetSharedAccessSignature(sasConstraints);
            return string.Concat(blob.Uri, token);
        }

        public async Task CreateBlobContainer(string containerName)
        {
            await PreapreBLobContainer(containerName);
        }

        public async Task UploadBlob(string containerName, string blobName, string filePath)
        {
            var blockBlob = await PrepareClourBlockBlob(containerName, blobName);
            using (var fileStream = File.OpenRead(filePath))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }
        }

        public async Task UploadFromStreamAsync(string containerName, string blobName, Stream stream)
        {
            var blockBlob = await PrepareClourBlockBlob(containerName, blobName);
            await blockBlob.UploadFromStreamAsync(stream);
        }

        public async Task DownloadToStreamAsync(string containerName, string blobName, Stream target)
        {
            var blockBlob = await PrepareClourBlockBlob(containerName, blobName);
            await blockBlob.DownloadToStreamAsync(target);
        }

        public async Task DownloadBlob(string containerName, string blobName, string filePath)
        {
            var blockBlob = await PrepareClourBlockBlob(containerName, blobName);
            using (var fileStream = System.IO.File.OpenWrite(filePath))
            {
                await blockBlob.DownloadToStreamAsync(fileStream);
            }
        }

        public async Task<string> DownloadBlob(string containerName, string blobName)
        {
            var blockBlob = await PrepareClourBlockBlob(containerName, blobName);
            using (var memoryStream = new MemoryStream())
            {
                blockBlob.DownloadToStream(memoryStream);
                return System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public async Task DeleteBlob(string containerName, string blobName)
        {
            var blockBlob = await PrepareClourBlockBlob(containerName, blobName);
            await blockBlob.DeleteAsync();
        }

        public async Task AppendBlob(string containerName, string blobName, string content)
        {
            var blobContainer = await PreapreBLobContainer(containerName);
            CloudAppendBlob appendBlob = blobContainer.GetAppendBlobReference(blobName);
            await appendBlob.CreateOrReplaceAsync();
            await appendBlob.AppendTextAsync(content);
        }

        public async Task<IList<string>> ListBlobs(string containerName)
        {
            var blobContainer = await PreapreBLobContainer(containerName);
            await blobContainer.CreateIfNotExistsAsync();
            return blobContainer.ListBlobs(null, false).Select(x => x.Uri.AbsolutePath).ToList();
        }

        private async Task<CloudBlockBlob> PrepareClourBlockBlob(string containerName, string blobName)
        {
            var blobContainer = await PreapreBLobContainer(containerName);
            await blobContainer.CreateIfNotExistsAsync();
            return blobContainer.GetBlockBlobReference(blobName);
        }

        private async Task<CloudBlobContainer> PreapreBLobContainer(string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(this.config.StorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerName);
            await blobContainer.CreateIfNotExistsAsync();
            return blobContainer;
        }
    }
}
