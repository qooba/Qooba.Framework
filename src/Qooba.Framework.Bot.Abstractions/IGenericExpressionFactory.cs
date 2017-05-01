using System;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IGenericExpressionFactory
    {
        object Create<TInterface>(string itemType, Func<string, TInterface> itemFactory, IConversationContext conversationContext, string itemDataText);
    }
}
