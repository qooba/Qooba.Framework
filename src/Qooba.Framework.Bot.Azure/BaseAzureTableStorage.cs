using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;

namespace Qooba.Framework.Bot.Azure
{
    public abstract class BaseAzureTableStorage
    {
        protected abstract string ConnectionString { get; }

        protected CloudTable PrepareTable(string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);
            return table;
        }
    }
}