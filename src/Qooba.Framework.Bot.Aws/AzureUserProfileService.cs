using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Qooba.Framework.Bot.Aws
{
    public class AwsUserProfileService : BaseAzureTableStorage, IUserProfileService
    {
        private readonly IBotConfig config;

        public AwsUserProfileService(IBotConfig config)
        {
            this.config = config;
        }

        protected override string ConnectionString => this.config.BotUserProfileConnectionString;

        public async Task<User> GetUserAsync(ConnectorType connectorType, string id)
        {
            var retrieveOperation = TableOperation.Retrieve<AzureUser>(connectorType.ToString(), id);
            var result = await this.PrepareTable(this.config.BotUserProfileTableName).ExecuteAsync(retrieveOperation);
            var user = (AzureUser)result.Result;
            if (user == null)
            {
                return null;
            }

            return new User
            {
                ConnectorType = Enum.TryParse(user.ConnectorType, out ConnectorType ct) ? ct : ConnectorType.Messanger,
                FirstName = user.FirstName,
                Gender = Enum.TryParse(user.Gender, out Gender gender) ? gender : Gender.Unknown,
                Id = user.Id,
                LastName = user.LastName,
                Locale = user.Locale,
                ProfilePicture = user.ProfilePicture,
                Timezone = user.Timezone
            };
        }

        public async Task SetUserAsync(User user)
        {
            var azureUser = new AzureUser
            {
                Gender = user.Gender.ToString(),
                ConnectorType = user.ConnectorType.ToString(),
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Locale = user.Locale,
                PartitionKey = user.ConnectorType.ToString(),
                ProfilePicture = user.ProfilePicture,
                RowKey = user.Id,
                Timezone = user.Timezone
            };

            var insertOperation = TableOperation.InsertOrReplace(azureUser);
            await this.PrepareTable(this.config.BotUserProfileTableName).ExecuteAsync(insertOperation);
        }
    }

    public class AzureUser : TableEntity
    {
        public AzureUser() { }

        public AzureUser(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePicture { get; set; }

        public string Locale { get; set; }

        public int Timezone { get; set; }

        public string Gender { get; set; }

        public string ConnectorType { get; set; }
    }
}