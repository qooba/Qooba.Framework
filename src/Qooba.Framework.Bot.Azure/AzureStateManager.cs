using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System;

namespace Qooba.Framework.Bot.Azure
{
    public class AzureStateManager : IStateManager
    {
        private readonly IBotConfig config;
        
        public AzureStateManager(IBotConfig config)
        {
            this.config = config;
        }

        public async Task<IConversationContext> FetchContextAsync(IConversationContext context)
        {
            var userId = context.Entry.Message.Sender.Id;
            var connectorType = context.ConnectorType.ToString();
            var insertOperation = TableOperation.Retrieve(connectorType, userId);
            var result = await this.PrepareTable(this.config.BotConversationContextTableName).ExecuteAsync(insertOperation);
            var lastContext = (IConversationContext)result.Result;

            if (lastContext.KeepState)
            {
                context.Route = lastContext.Route;
            }

            return context;
        }
        
        public async Task SaveContextAsync(IConversationContext context)
        {
            var insertOperation = TableOperation.Insert(new AzureConversationContext(context));
            await this.PrepareTable(this.config.BotConversationContextTableName).ExecuteAsync(insertOperation);
        }
        
        private CloudTable PrepareTable(string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(this.config.BotStateManagerConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);
            return table;
        }
    }

    public class AzureConversationContext : TableEntity, IConversationContext
    {
        public AzureConversationContext(IConversationContext context)
        {
            this.PartitionKey = context.ConnectorType.ToString();
            this.RowKey = context.Entry.Message.Sender.Id;
            this.Route = context.Route;
            this.User = context.User;
            this.ConnectorType = context.ConnectorType;
            this.Entry = context.Entry;
            this.Reply = context.Reply;
            this.KeepState = context.KeepState;
        }

        public Route Route { get; set; }

        public User User { get; set; }

        public ConnectorType ConnectorType { get; set; }

        public Entry Entry { get; set; }

        public Reply Reply { get; set; }

        public bool KeepState { get; set; }
    }
}