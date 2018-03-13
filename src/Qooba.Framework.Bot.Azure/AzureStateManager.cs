using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System.Net;
using Qooba.Framework.Abstractions;

namespace Qooba.Framework.Bot.Azure
{
    public class AzureStateManager : BaseAzureTableStorage, IStateManager
    {
        private readonly IBotConfig config;

        private readonly ISerializer serializer;

        public AzureStateManager(IBotConfig config, ISerializer serializer)
        {
            this.config = config;
            this.serializer = serializer;
        }

        protected override string ConnectionString => this.config.BotStateManagerConnectionString;

        public async Task ClearContextAsync(IConversationContext context)
        {
            try
            {
                var azureContext = new AzureConversationContext
                {
                    PartitionKey = context.ConnectorType.ToString(),
                    RowKey = context.Entry.Message.Sender.Id,
                    ETag = "*",
                    StateAction = context.StateAction
                };

                var deleteOperation = TableOperation.Delete(azureContext);
                await this.PrepareTable(this.config.BotConversationContextTableName).ExecuteAsync(deleteOperation);
            }
            catch (StorageException e)
            {
                if (e.RequestInformation.HttpStatusCode == (int)HttpStatusCode.NotFound)
                {
                    return;
                }

                throw;
            }
        }

        public async Task<IConversationContext> FetchContextAsync(IConversationContext context)
        {
            var userId = context.Entry.Message.Sender.Id;
            var connectorType = context.ConnectorType.ToString();
            var retrieveOperation = TableOperation.Retrieve<AzureConversationContext>(connectorType, userId);
            var result = await this.PrepareTable(this.config.BotConversationContextTableName).ExecuteAsync(retrieveOperation);
            var lastContext = (AzureConversationContext)result.Result;

            if (lastContext != null)
            {
                var data = this.serializer.Deserialize<AzureConversationContext>(lastContext.ContextData);
                context.Route = data.Route;
                context.Reply = data.Reply;
            }

            return context;
        }

        public async Task SaveContextAsync(IConversationContext context)
        {
            var contextData = this.serializer.Serialize(context);
            var azureContext = new AzureConversationContext
            {
                PartitionKey = context.ConnectorType.ToString(),
                RowKey = context.Entry.Message.Sender.Id,
                StateAction = context.StateAction,
                ContextData = contextData
            };

            var insertOperation = TableOperation.InsertOrReplace(azureContext);
            await this.PrepareTable(this.config.BotConversationContextTableName).ExecuteAsync(insertOperation);
        }
    }

    public class AzureConversationContext : TableEntity, IConversationContext
    {
        public AzureConversationContext() { }

        public AzureConversationContext(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public string ContextData { get; set; }

        public Route Route { get; set; }

        public User User { get; set; }

        public ConnectorType ConnectorType { get; set; }

        public Entry Entry { get; set; }

        public Reply Reply { get; set; }

        public StateAction StateAction { get; set; }
    }
}