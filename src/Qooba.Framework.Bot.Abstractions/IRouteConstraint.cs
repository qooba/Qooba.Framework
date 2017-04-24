using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IRouteConstraint
    {
        bool Match(IConversationContext conversationContext, string parameterName, IDictionary<string, object> values);
    }
}