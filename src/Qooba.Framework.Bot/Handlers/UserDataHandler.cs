using Qooba.Framework.Bot.Abstractions;
using System;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Handlers
{
    public class UserDataHandler : BaseHandler, IHandler
    {
        private readonly Func<object, IUserManager> userManagerFunc;

        public UserDataHandler(Func<object, IUserManager> userManagerFunc)
        {
            this.userManagerFunc = userManagerFunc;
        }

        public override int Priority => 1;

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            var userManager = this.userManagerFunc(conversationContext.ConnectorType);
            conversationContext.User = await userManager.GetUserAsync(conversationContext.Entry.Message.Sender.Id);
            await base.InvokeAsync(conversationContext);
        }
    }
}