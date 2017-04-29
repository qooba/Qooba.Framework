using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections.Generic;
using System;

namespace Qooba.Framework.Bot
{
    public class FormReplyBuilder : IReplyBuilder<FormReplyMessage>
    {
        private readonly IReplyFactory replyFactory;

        private readonly Func<object, IFormReplyPropertyActiveConstraint> activeConstraintFactory;

        private readonly Func<object, IFormReplyPropertyValidator> validatorFactory;

        private readonly Func<object, IFormReplyCompletionAction> completionActionFactory;

        public FormReplyBuilder(IReplyFactory replyFactory)
        {
            this.replyFactory = replyFactory;
        }

        public async Task<ReplyMessage> BuildAsync(IConversationContext conversationContext, FormReplyMessage reply)
        {
            foreach(var property in reply.Properties)
            {
                if(!conversationContext.Route.RouteData.ContainsKey(property.PropertyName))
                {
                    //TODO: check if the last field is valid

                    //TODO: check if field is active

                    return (await this.replyFactory.CreateReplyAsync(conversationContext, property.ReplyItem)).Message;
                }
            }

            //TODO: execute completion action
            return new ReplyMessage
            {
                Text = "Form completed"
            };
        }
    }

    //private void Validate(IConversationContext conversationContext)
    //{
    //    foreach(var validator in )

    //    var builder = validatorFactory(replyItem.ReplyType);
    //    Func<IReplyBuilder, IConversationContext, object, Task<ReplyMessage>> builderFunc = null;

    //    if (!cachedFunc.TryGetValue(replyItem.ReplyType, out builderFunc))
    //    {
    //        foreach (var i in builder.GetType().GetTypeInfo().GetInterfaces())
    //        {
    //            if (i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IReplyBuilder<>))
    //            {
    //                var type = i.GetGenericArguments().FirstOrDefault();
    //                var method = i.GetMethods().FirstOrDefault(x => x.Name == "BuildAsync");
    //                cachedReplyType[replyItem.ReplyType] = type;
    //                var build = Expression.Parameter(typeof(IReplyBuilder), "builder");
    //                var context = Expression.Parameter(typeof(IConversationContext), "conversationContext");
    //                var reply = Expression.Parameter(typeof(object), "reply");

    //                var builderFuncExpression = Expression.Lambda<Func<IReplyBuilder, IConversationContext, object, Task<ReplyMessage>>>(Expression.Call(Expression.Convert(build, i), method, context, Expression.Convert(reply, type)), build, context, reply);
    //                builderFunc = builderFuncExpression.Compile();
    //                cachedFunc[replyItem.ReplyType] = builderFunc;
    //                break;
    //            }
    //        }
    //    }
    //}

    public class FormReplyMessage
    {
        public IEnumerable<FormReplyMessageProperty> Properties { get; set; }

        public IEnumerable<CompletionAction> CompletionActions { get; set; }
    }

    public class FormReplyMessageProperty
    {
        public string PropertyName { get; set; }

        public string PropertyType { get; set; }
        
        public IEnumerable<ActiveConstraint> ActiveConstraints { get; set; }

        public IEnumerable<Validator> Validators { get; set; }
        
        public ReplyItem ReplyItem { get; set; }
    }

    public class ActiveConstraint
    {
        public string ActiveConstraintType { get; set; }

        public object ActiveConstraintData { get; set; }
    }

    public class Validator
    {
        public string ValidatorType { get; set; }

        public object ValidatorData { get; set; }
    }

    public class CompletionAction
    {
        public string CompletionActionType { get; set; }

        public object CompletionActionData { get; set; }
    }

    public interface IFormReplyPropertyActiveConstraint
    {

    }

    public interface IFormReplyPropertyActiveConstraint<T>
        where T : class
    {
        /// <summary>
        /// Check if the property will be visible
        /// </summary>
        /// <param name="conversationContext"></param>
        /// <returns></returns>
        Task<bool> CheckActiveAsync(IConversationContext conversationContext);
    }

    public interface IFormReplyPropertyValidator
    {

    }

    public interface IFormReplyPropertyValidator<T>
        where T : class
    {
        /// <summary>
        /// Check is the response is valid if is invalid return ReplyItem
        /// </summary>
        /// <param name="conversationContext"></param>
        /// <returns></returns>
        Task<ReplyItem> CheckValidAsync(IConversationContext conversationContext);
    }

    public interface IFormReplyCompletionAction
    {

    }

    public interface IFormReplyCompletionAction<T>
        where T : class
    {
        /// <summary>
        /// Action which will be executed when the form is complete
        /// </summary>
        /// <param name="conversationContext"></param>
        /// <returns></returns>
        Task<ReplyItem> ExecuteAsync(IConversationContext conversationContext);
    }
}