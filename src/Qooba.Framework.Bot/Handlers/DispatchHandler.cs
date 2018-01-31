using Qooba.Framework.Bot.Abstractions;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Qooba.Framework.Bot.Common;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot.Handlers
{
    public class DispatchHandler : BaseHandler, IHandler
    {
        private readonly Func<object, IDispatcher> replyClientFunc;

        public DispatchHandler(Func<object, IDispatcher> replyClientFunc)
        {
            this.replyClientFunc = replyClientFunc;
        }

        public override int Priority => 4;

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            var replyClient = this.replyClientFunc(conversationContext.ConnectorType);

            if (conversationContext.Reply != null)
            {
                PickReplyText(conversationContext.Reply);
                await replyClient.SendAsync(conversationContext.Reply);
            }

            await base.InvokeAsync(conversationContext);
        }

        private void PickReplyText(Reply reply)
        {
            var text = reply?.Message?.Text;
            if (text != null)
            {
                var matches = Regex.Matches($"{text}", @"\[[a-zA-Z0-9śćźżłóę \.\-|]+\]", RegexOptions.IgnoreCase);
                foreach (var match in matches)
                {
                    var t = match.ToString();
                    var tt = t.TrimStart('[').TrimEnd(']').Split('|').PickRandom();
                    text = text.Replace(t, tt);
                }

                reply.Message.Text = text;
            }
        }
    }
}