using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Form;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Serialization.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Form
{
    public class HttpFormReplyCompletionAction : IFormReplyCompletionAction<HttpFormReplyCompletionActionData>
    {
        private readonly ISerializer serializer;

        public HttpFormReplyCompletionAction(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        public async Task<ReplyMessage> ExecuteAsync(IConversationContext conversationContext, HttpFormReplyCompletionActionData completionActionData)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(completionActionData.Url, conversationContext);
                var responseString = await response.Content.ReadAsStringAsync();
                return this.serializer.Deserialize<ReplyMessage>(responseString);
            }
        }
    }

    public class HttpFormReplyCompletionActionData
    {
        public string Url { get; set; }
    }
}
