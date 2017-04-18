using Qooba.Framework.Bot.Abstractions.Models;
using System;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IReplyBuilder<T> : IReplyBuilder
        where T : class
    {
        Task<ReplyMessage> BuildAsync(IConversationContext context, T reply);
    }

    public interface IReplyBuilder
    {

    }
}

