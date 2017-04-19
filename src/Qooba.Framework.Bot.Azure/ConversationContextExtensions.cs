using Qooba.Framework.Bot.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Azure
{
    public static class ConversationContextExtensions
    {
        public static Task<T> FetchUserDataAsync<T>(this IConversationContext conversationContext, string key)
            where T : class
        {
            return null;
        }

        public static Task<T> FetchConversationDataAsync<T>(this IConversationContext conversationContext, string key)
            where T : class
        {
            return null;
        }

        public static Task<T> FetchPrivateConversationDataAsync<T>(this IConversationContext conversationContext, string key)
            where T : class
        {
            return null;
        }

        public static Task SaveUserDataAsync<T>(this IConversationContext conversationContext, string key, T data)
            where T : class
        {
            return null;
        }

        public static Task SaveConversationDataAsync<T>(this IConversationContext conversationContext, string key, T data) where T : class
        {
            return null;
        }

        public static Task SavePrivateConversationDataAsync<T>(this IConversationContext conversationContext, string key, T data)
            where T : class
        {
            return null;
        }
    }
}
