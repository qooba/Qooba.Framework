using Microsoft.Extensions.DependencyInjection;
using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.DependencyInjection.DefaultContainer;
using System;

namespace Qooba.Framework.DependencyInjection.SimpleContainer
{
    public class DefaultContainerModule : IModule
    {
        public virtual string Name
        {
            get { return "DefaultContainerModule"; }
        }
        
        public int Priority
        {
            get { return 0; }
        }

        public void Bootstrapp()
        {
            var container = new ContainerWrapper(new ServiceCollection());
            container.RegisterInstance<IContainer>(container);
            ContainerManager.SetContainer(container);
        }
    }
}
