using Qooba.Framework.Bot.Abstractions.Models;
using System;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IReplyBuilder
    {
        Task<ReplyMessage> BuildAsync(IConversationContext context, object reply);

        Type ReplyItemType { get; }
    }
}

