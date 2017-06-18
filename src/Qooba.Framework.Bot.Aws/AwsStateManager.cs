using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Serialization.Abstractions;
using Amazon.DynamoDBv2;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using System;

namespace Qooba.Framework.Bot.Aws
{
    public class AwsStateManager : IStateManager
    {
        private readonly IBotConfig config;

        private readonly ISerializer serializer;

        private readonly IAmazonDynamoDB client;

        public AwsStateManager(IBotConfig config, ISerializer serializer)
        {
            this.config = config;
            this.serializer = serializer;
            this.client = new AmazonDynamoDBClient();
        }

        public async Task ClearContextAsync(IConversationContext context)
        {
            var request = new DeleteItemRequest
            {
                TableName = this.config.BotConversationContextTableName,
                Key = new Dictionary<string, AttributeValue>() { { "Id", new AttributeValue { SS = new List<string> { context.ConnectorType.ToString(), context.Entry.Message.Sender.Id } } } }
            };

            await client.DeleteItemAsync(request);
        }

        public async Task<IConversationContext> FetchContextAsync(IConversationContext context)
        {
            var request = new GetItemRequest
            {
                TableName = this.config.BotConversationContextTableName,
                Key = new Dictionary<string, AttributeValue>() { { "Id", new AttributeValue { SS = new List<string> { context.ConnectorType.ToString(), context.Entry.Message.Sender.Id } } } }
            };

            var response = await client.GetItemAsync(request);

            AttributeValue contextData = null;
            if (response.IsItemSet && response.Item.TryGetValue("ContextData", out contextData))
            {
                var data = this.serializer.Deserialize<AwsConversationContext>(contextData.S);
                context.Route = data.Route;
                context.Reply = data.Reply;
            }

            return context;
        }

        public async Task SaveContextAsync(IConversationContext context)
        {
            var contextData = this.serializer.Serialize(context);
            var request = new PutItemRequest
            {
                TableName = this.config.BotConversationContextTableName,
                Item = new Dictionary<string, AttributeValue>()
                {
                    { "Id", new AttributeValue { SS = new List<string> { context.ConnectorType.ToString(), context.Entry.Message.Sender.Id } } },
                    { "ContextData", new AttributeValue { S = contextData }}
                }
            };

            await client.PutItemAsync(request);
        }
    }

    public class AwsConversationContext : IConversationContext
    {
        public AwsConversationContext() { }

        public string ContextData { get; set; }

        public Route Route { get; set; }

        public User User { get; set; }

        public ConnectorType ConnectorType { get; set; }

        public Entry Entry { get; set; }

        public Reply Reply { get; set; }

        public StateAction StateAction { get; set; }
    }
}