using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;

namespace Qooba.Framework.ServiceFabric
{
    public class ServiceFabricModule : IModule
    {
        public virtual string Name
        {
            get { return "ServiceFabricModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            //ContainerManager.Current.RegisterType<ISerializer, JsonSerializer>();
        }
    }
}
