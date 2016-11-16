using Qooba.Framework.Abstractions;
using Qooba.Framework.Configuration;
using Qooba.Framework.Configuration.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;

namespace Qooba.Framework.Azure
{
    public class ConfigModule : IModule
    {
        public virtual string Name
        {
            get { return "ConfigModule"; }
        }
        
        public int Priority
        {
            get { return 2; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<IConfig, Config>(Lifetime.Singleton);
        }
    }
}
