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
        private static readonly IDictionary<Type, FormReplyMessage> formReplyMessageCache = new ConcurrentDictionary<Type, FormReplyMessage>();

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
                    .Select(x => new CompletionAction { CompletionActionType = x.TypeKey, CompletionActionData = x.Data });

                var properties = modelTypeInfo.GetProperties();
                var messageProperties = new List<FormReplyMessageProperty>();
                foreach (var property in properties)
                {
                    var propertyAttributes = property.GetCustomAttributes();

                    var replyType = (PropertyReplyAttribute)propertyAttributes.FirstOrDefault(x => x is PropertyReplyAttribute);
                    var validators = propertyAttributes.Select(x => x as PropertyValidatorAttribute).Where(x => x != null);
                    var confirmations = propertyAttributes.Select(x => x as PropertyConfirmAttribute).Where(x => x != null);

                    var prop = new FormReplyMessageProperty
                    {
                        PropertyName = property.Name,
                        PropertyType = property.PropertyType.ToString(),
                        ReplyItem = new Abstractions.Models.ReplyItem
                        {
                            ReplyId = replyType.TypeKey,
                            ReplyType = replyType.TypeKey,
                            Reply = replyType.Data
                        },
                        Validators = validators.Select(x => new Validator { ValidatorType = x.TypeKey, ValidatorData = x.Data }),
                        Confirmations = confirmations.Select(x => new Confirm { ConfirmType = x.TypeKey, ConfirmData = x.Data })
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
