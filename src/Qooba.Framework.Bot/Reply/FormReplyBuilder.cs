using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Qooba.Framework.Serialization.Abstractions;
using Qooba.Framework.Bot.Abstractions.Form;

namespace Qooba.Framework.Bot
{
    public class FormReplyBuilder : IReplyBuilder<FormReplyMessage>
    {
        private readonly IReplyFactory replyFactory;

        private readonly IEnumerable<IRouter> routers;

        private readonly IGenericExpressionFactory genericExpressionFactory;

        private readonly Func<object, IFormReplyPropertyActiveConstraint> activeConstraintFactory;

        private readonly Func<object, IFormReplyPropertyValidator> validatorFactory;

        private readonly Func<object, IFormReplyCompletionAction> completionActionFactory;

        public FormReplyBuilder(
            IReplyFactory replyFactory,
            IEnumerable<IRouter> routers,
            IGenericExpressionFactory genericExpressionFactory,
            Func<object, IFormReplyCompletionAction> completionActionFactory
            )
        {
            this.replyFactory = replyFactory;
            this.routers = routers;
            this.genericExpressionFactory = genericExpressionFactory;
            this.completionActionFactory = completionActionFactory;
        }

        public async Task<ReplyMessage> ExecuteAsync(IConversationContext conversationContext, FormReplyMessage reply)
        {
            for (var i = 0; i < reply.Properties.Count(); i++)
            {
                conversationContext.StateAction = StateAction.Keep;
                var property = reply.Properties.ElementAt(i);
                var propertyName = property.PropertyName.ToLower();
                if (!conversationContext.Route.RouteData.ContainsKey(propertyName))
                {
                    //TODO: check if the last field is valid

                    if (conversationContext.Reply != null)
                    {
                        if (property.ReplyItem.Routes != null && property.ReplyItem.Routes.Any())
                        {
                            foreach (var router in this.routers)
                            {
                                var routeData = await router.FindRouteData(conversationContext.Entry.Message.Message.Text, property.ReplyItem.Routes);
                                if (routeData != null && routeData.ContainsKey(propertyName))
                                {
                                    conversationContext.Route.RouteData[propertyName] = routeData[propertyName];
                                }
                            }
                        }
                        else
                        {
                            conversationContext.Route.RouteData[propertyName] = conversationContext.Entry.Message.Message.Text;
                        }

                        i++;
                        if (reply.Properties.Count() == i)
                        {
                            break;
                        }
                        else
                        {
                            property = reply.Properties.ElementAt(i);
                        }
                    }

                    //TODO: check if field is active

                    return (await this.replyFactory.CreateReplyAsync(conversationContext, property.ReplyItem)).Message;
                }
            }

            conversationContext.StateAction = StateAction.Clear;
            return await this.Complete(conversationContext, reply);
        }

        private async Task<ReplyMessage> Complete(IConversationContext conversationContext, FormReplyMessage formReplyMessage)
        {
            foreach (var completionAction in formReplyMessage.CompletionActions)
            {
                var message = await (Task<ReplyMessage>)this.genericExpressionFactory.Create(completionAction.CompletionActionType, completionActionFactory, conversationContext, completionAction.CompletionActionData.ToString());

                if (message != null)
                {
                    return message;
                }
            }

            return null;
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
}