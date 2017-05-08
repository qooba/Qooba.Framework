using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Serialization.Abstractions;
using System.Net.Http;

namespace Qooba.Framework.Bot
{
    public class HttpReplyBuilder : IReplyBuilder<HttpReplyMessage>
    {
        private readonly ISerializer serializer;

        public HttpReplyBuilder(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        public async Task<ReplyMessage> ExecuteAsync(IConversationContext context, HttpReplyMessage reply)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(reply.Url, context);
                var responseString = await response.Content.ReadAsStringAsync();
                return this.serializer.Deserialize<ReplyMessage>(responseString);
            }
        }
    }

    public class HttpReplyMessage
    {
        public string Url { get; set; }
    }
}
