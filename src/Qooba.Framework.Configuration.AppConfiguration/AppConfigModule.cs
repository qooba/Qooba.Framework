using Qooba.Framework.Abstractions;
using Qooba.Framework.Configuration;
using Qooba.Framework.Configuration.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;

namespace Qooba.Framework.Configuration.AppConfiguration
{
    public class AppConfigModule : IModule
    {
        public virtual string Name
        {
            get { return "AppConfigModule"; }
        }
        
        public int Priority
        {
            get { return 2; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<IConfig, AppConfig>(Lifetime.Singleton);
        }
    }
}
