using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;

namespace Qooba.Framework.DependencyInjection.SimpleContainer
{
    public class SimpleContainerModule : IModule
    {
        public virtual string Name
        {
            get { return "SimpleContainerModule"; }
        }
        
        public int Priority
        {
            get { return 0; }
        }

        public void Bootstrapp()
        {
            var container = new Container();
            container.RegisterInstance<IContainer>(container);
            ContainerManager.SetContainer(container);
        }
    }
}
