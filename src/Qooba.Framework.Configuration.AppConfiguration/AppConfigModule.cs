using Qooba.Framework.Abstractions;
using Qooba.Framework.Configuration.Abstractions;

namespace Qooba.Framework.Configuration.AppConfiguration
{
    public class AppConfigModule : IModule
    {
        public virtual string Name => "AppConfigModule";

        public int Priority => 2;

        public void Bootstrapp(IContainer container)
        {
            container.RegisterType<IConfig, AppConfig>(Lifetime.Singleton);
        }
    }
}
