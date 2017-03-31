using System;
using Qooba.Framework.Abstractions;

namespace Qooba.Framework.DependencyInjection.SimpleContainer
{
    public class SimpleContainerModule : IServiceBootstrapper
    {
        public virtual string Name => "SimpleContainerModule";

        public int Priority => 0;
        
        public void Bootstrapp(IContainer container)
        {
        }

        public IContainer BootstrappContainer()
        {
            var container = new Container();
            container.RegisterInstance<IContainer>(container);
            return container;
        }
    }
}
