using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections.Generic;
using System;
using System.Linq;
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

        private readonly Func<object, IFormReplyPropertyConfirmation> confirmationFactory;

        private readonly Func<object, IFormReplyCompletionAction> completionActionFactory;

        public FormReplyBuilder(
            IReplyFactory replyFactory,
            IEnumerable<IRouter> routers,
            IGenericExpressionFactory genericExpressionFactory,
            Func<object, IFormReplyCompletionAction> completionActionFactory,
            Func<object, IFormReplyPropertyActiveConstraint> activeConstraintFactory,
            Func<object, IFormReplyPropertyValidator> validatorFactory,
            Func<object, IFormReplyPropertyConfirmation> confirmationFactory
            )
        {
            this.replyFactory = replyFactory;
            this.routers = routers;
            this.genericExpressionFactory = genericExpressionFactory;
            this.completionActionFactory = completionActionFactory;
            this.activeConstraintFactory = activeConstraintFactory;
            this.validatorFactory = validatorFactory;
            this.confirmationFactory = confirmationFactory;
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
                    var validationMessage = await this.CheckValid(conversationContext, reply, property);
                    if (validationMessage != null)
                    {
                        return validationMessage;
                    }

                    //TODO: add confirmation propperty

                    if (conversationContext.Reply != null && conversationContext.Entry?.Message?.Message != null)
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
                            if (conversationContext.Entry.Message.Message?.Quick_replies != null)
                            {
                                conversationContext.Route.RouteData[$"{Constants.QuickRepliesProperty}::{propertyName}"] = conversationContext.Entry.Message.Message?.Quick_replies.FirstOrDefault();
                            }
                        }

                        conversationContext.Entry.Message.Message = null;
                        continue;
                    }

                    if (!await this.CheckActive(conversationContext, reply, property))
                    {
                        continue;
                    }

                    var createdReply = (await this.replyFactory.CreateReplyAsync(conversationContext, property.ReplyItem)).Message;
                    if (createdReply == null)
                    {
                        continue;
                    }

                    return createdReply;
                }
            }

            conversationContext.StateAction = StateAction.Clear;
            return await this.Complete(conversationContext, reply);
        }

        private async Task<bool> CheckActive(IConversationContext conversationContext, FormReplyMessage formReplyMessage, FormReplyMessageProperty property)
        {
            if (property?.ActiveConstraints != null)
            {
                foreach (var activeConstraint in property.ActiveConstraints)
                {
                    var isActive = await (Task<bool>)this.genericExpressionFactory.Create(activeConstraint.ActiveConstraintType, this.activeConstraintFactory, conversationContext, activeConstraint.ActiveConstraintData.ToString());
                    if (!isActive)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private async Task<ReplyMessage> CheckValid(IConversationContext conversationContext, FormReplyMessage formReplyMessage, FormReplyMessageProperty property)
        {
            if (property?.Validators != null)
            {
                foreach (var validator in property.Validators)
                {
                    var validationMessage = await (Task<ReplyMessage>)this.genericExpressionFactory.Create(validator.ValidatorType, this.validatorFactory, conversationContext, validator.ValidatorData.ToString());
                    if (validationMessage != null)
                    {
                        return validationMessage;
                    }
                }
            }

            return null;
        }

        private async Task<ReplyMessage> Complete(IConversationContext conversationContext, FormReplyMessage formReplyMessage)
        {
            foreach (var completionAction in formReplyMessage.CompletionActions)
            {
                var message = await (Task<ReplyMessage>)this.genericExpressionFactory.Create(completionAction.CompletionActionType, this.completionActionFactory, conversationContext, completionAction.CompletionActionData.ToString());

                if (message != null)
                {
                    return message;
                }
            }

            return null;
        }
    }

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

        public IEnumerable<Confirm> Confirmations { get; set; }

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

    public class Confirm
    {
        public string ConfirmType { get; set; }

        public object ConfirmData { get; set; }
    }

    public class CompletionAction
    {
        public string CompletionActionType { get; set; }

        public object CompletionActionData { get; set; }
    }
}