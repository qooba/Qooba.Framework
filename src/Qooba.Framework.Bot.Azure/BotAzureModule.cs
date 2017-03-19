using Qooba.Framework.Abstractions;
using Qooba.Framework.Bot.Abstractions;

namespace Qooba.Framework.Bot.Azure
{
    public class BotAzureModule : IModule
    {
        public virtual string Name => "BotAzureModule";

        public int Priority => 20;

        public void Bootstrapp(IContainer container)
        {
            container.RegisterType<IStateManager, AzureStateManager>();
            container.RegisterType<IMessageQueue,AzureMessageQueue>();
        }
    }
}
