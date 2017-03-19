using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Azure.Storage.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace Qooba.Framework.Bot.Azure
{
    public class AzureStateManager : IStateManager
    {
        private readonly IAzureBlobTableRepository<AzureConversationContext> repository;

        public AzureStateManager(IAzureBlobTableRepository<AzureConversationContext> repository)
        {
            this.repository = repository;
        }

        public async Task<IConversationContext> FetchContext(IConversationContext context)
        {
            var userId = context.Entry.Message.Sender.Id;
            var connectorType = context.ConnectorType.ToString();

            var lastContext = await this.repository.FirstOrDefault(connectorType, userId);
            if (lastContext.KeepState)
            {
                context.Route = lastContext.Route;
            }

            return context;
        }

        public async Task SaveContext(IConversationContext context)
        {
            await this.repository.Add(new AzureConversationContext(context));
        }
    }

    public class AzureConversationContext :  TableEntity, IConversationContext
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