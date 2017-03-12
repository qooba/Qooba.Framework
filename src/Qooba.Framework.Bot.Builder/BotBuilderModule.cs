using Qooba.Framework.Abstractions;

namespace Qooba.Framework.Bot.Builder
{
    public class BotBuilderModule : IModule
    {
        public virtual string Name => "ConfigModule";

        public int Priority => 2;

        public void Bootstrapp(IContainer container)
        {
            //ContainerManager.Current.RegisterType<IConfig, Config>(Lifetime.Singleton);
        }
    }
}
