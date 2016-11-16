using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.Services.Abstractions;
using System;

namespace Qooba.Framework.Services
{
    public class ServicesModule : IModule
    {
        public virtual string Name
        {
            get { return "ServicesModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<IRestServiceFactory, RestServiceFactory>();
        }
    }
}
