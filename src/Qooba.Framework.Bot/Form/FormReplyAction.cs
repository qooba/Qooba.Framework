using System.Collections.Generic;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq;
using Qooba.Framework.Bot.Attributes;

namespace Qooba.Framework.Bot.Form
{
    public class FormReplyAction<TModel> : IReplyAction<FormReplyMessage>
        where TModel : class
    {
        private readonly IDictionary<Type, FormReplyMessage> formReplyMessageCache = new ConcurrentDictionary<Type, FormReplyMessage>();

        public async Task<FormReplyMessage> CreateReplyMessage(IConversationContext conversationContext, IDictionary<string, string> parameters)
        {
            var modelType = typeof(TModel);
            if (!formReplyMessageCache.TryGetValue(modelType, out FormReplyMessage formReplyMessage))
            {
                formReplyMessage = new FormReplyMessage();
                var modelTypeInfo = modelType.GetTypeInfo();
                var attributes = modelTypeInfo.GetCustomAttributes();
                formReplyMessage.CompletionActions = attributes
                    .Select(x => (x as CompletionActionAttribute))
                    .Where(x => x != null)
                    .Select(x => new CompletionAction { CompletionActionType = x.CompletionActionType, CompletionActionData = x.CompletionActionData });

                var properties = modelTypeInfo.GetProperties();
                var messageProperties = new List<FormReplyMessageProperty>();
                foreach (var property in properties)
                {
                    var propertyTypeInfo = property.GetType().GetTypeInfo();
                    var propertyAttributes = propertyTypeInfo.GetCustomAttributes();

                    var replyType = (PropertyReplyAttribute)propertyAttributes.FirstOrDefault(x => x is PropertyReplyAttribute);

                    var prop = new FormReplyMessageProperty
                    {
                        PropertyName = propertyTypeInfo.Name,
                        PropertyType = propertyTypeInfo.ToString(),
                        ReplyItem = new Abstractions.Models.ReplyItem
                        {
                            ReplyId = replyType.ReplyType,
                            ReplyType = replyType.ReplyType,
                            Reply = replyType.Reply
                        }
                    };

                    messageProperties.Add(prop);
                }

                formReplyMessage.Properties = messageProperties;
                formReplyMessageCache[modelType] = formReplyMessage;
            }

            return formReplyMessage;
        }
    }
}
