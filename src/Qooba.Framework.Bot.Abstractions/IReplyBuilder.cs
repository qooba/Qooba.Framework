using Qooba.Framework.Bot.Abstractions.Models;
using System;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IReplyBuilder
    {
        Task<Reply> BuildAsync(IConversationContext context, ReplyItem replyItem);

        Type ReplyItemType { get; }
    }
}

