using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Aws
{
    public class AwsUserProfileService : IUserProfileService
    {
        private const string Id = "Id";

        private const string FirstName = "FirstName";

        private const string LastName = "LastName";

        private const string Gender = "Gender";

        private const string Locale = "Locale";

        private const string ProfilePicture = "ProfilePicture";

        private const string Timezone = "Timezone";

        private readonly IBotConfig config;

        private readonly IAmazonDynamoDB client;

        public AwsUserProfileService(IBotConfig config)
        {
            this.config = config;
            this.client = new AmazonDynamoDBClient();
        }

        public async Task<User> GetUserAsync(ConnectorType connectorType, string id)
        {
            var request = new GetItemRequest
            {
                TableName = this.config.BotUserProfileTableName,
                Key = new Dictionary<string, AttributeValue>() { { Id, new AttributeValue { SS = new List<string> { connectorType.ToString(), id } } } }
            };

            var response = await client.GetItemAsync(request);
            if (!response.IsItemSet)
            {
                return null;
            }

            var item = response.Item;

            return new User
            {
                ConnectorType = connectorType,
                FirstName = item.TryGetValue(FirstName, out AttributeValue firstName) ? firstName.S : null,
                Gender = item.TryGetValue(Gender, out AttributeValue gender) ? (Enum.TryParse(gender.S, out Gender gd) ? gd : Abstractions.Models.Gender.Unknown) : Abstractions.Models.Gender.Unknown,
                Id = id,
                LastName = item.TryGetValue(LastName, out AttributeValue lastName) ? lastName.S : null,
                Locale = item.TryGetValue(Locale, out AttributeValue locale) ? locale.S : null,
                ProfilePicture = item.TryGetValue(ProfilePicture, out AttributeValue profilePicutre) ? profilePicutre.S : null,
                Timezone = item.TryGetValue(Timezone, out AttributeValue timezone) ? (int.TryParse(timezone.N, out int tz) ? tz : 0) : 0
            };
        }

        public async Task SetUserAsync(User user)
        {

            var request = new PutItemRequest
            {
                TableName = this.config.BotUserProfileTableName,
                Item = new Dictionary<string, AttributeValue>()
                {
                    { Id, new AttributeValue { SS = new List<string> { user.ConnectorType.ToString(), user.Id } }},
                    { FirstName, new AttributeValue { S = user.FirstName }},
                    { LastName, new AttributeValue { S = user.LastName }},
                    { Gender, new AttributeValue { S = user.Gender.ToString() }},
                    { Locale, new AttributeValue { S = user.Locale }},
                    { ProfilePicture, new AttributeValue { S = user.ProfilePicture }},
                    { Timezone, new AttributeValue { N = user.Timezone.ToString() }},
                }
            };

            await client.PutItemAsync(request);
        }
    }
}