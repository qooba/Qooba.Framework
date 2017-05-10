using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Serialization.Abstractions;
using System.Net.Http;
using System.Text;

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
                var content = new StringContent(this.serializer.Serialize(context), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(reply.Url, content);
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
