using Microsoft.Extensions.DependencyInjection;
using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.DefaultContainer;

namespace Qooba.Framework.DependencyInjection.SimpleContainer
{
    public class DefaultContainerModule : IContainerBootstrapper
    {
        public virtual string Name => "DefaultContainerModule";

        public int Priority => 0;
        
        public void Bootstrapp(IContainer container)
        {
        }

        public IContainer BootstrappContainer()
        {
            var container = new ContainerWrapper(new ServiceCollection());
            container.RegisterInstance<IContainer>(container);
            return container;
        }
    }
}
