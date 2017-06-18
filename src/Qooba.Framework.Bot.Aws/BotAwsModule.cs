using Qooba.Framework.Abstractions;
using Qooba.Framework.Bot.Abstractions;

namespace Qooba.Framework.Bot.Aws
{
    public class BotAwsModule : IModule
    {
        public virtual string Name => "BotAwsModule";

        public int Priority => 20;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddTransientService<IStateManager, AwsStateManager>();
            framework.AddTransientService<IUserProfileService, AwsUserProfileService>();
            framework.AddTransientService<IMessageQueue, AwsMessageQueue>();
        }
    }
}
