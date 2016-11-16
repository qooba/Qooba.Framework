using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Qooba.Framework.Azure.Storage.Abstractions;
using Qooba.Framework.Configuration.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Azure.Storage
{
    public class AzureBlobTable : IAzureBlobTable
    {
        private readonly IConfig config;

        public AzureBlobTable(IConfig config)
        {
            this.config = config;
        }
        
        public async Task CreateTable(string tableName)
        {
            var table = PreapreTableReference(tableName);
            await table.CreateIfNotExistsAsync();
        }
        
        public async Task DeleteTable(string tableName)
        {
            var table = PreapreTableReference(tableName);
            await table.DeleteIfExistsAsync();
        }

        private CloudTable PreapreTableReference(string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(this.config.StorageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            return tableClient.GetTableReference(tableName);
        }
    }
}
