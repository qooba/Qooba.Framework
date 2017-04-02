using Microsoft.Extensions.DependencyInjection;
using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.DependencyInjection.DefaultContainer;

namespace Qooba.Framework.DependencyInjection.SimpleContainer
{
    public class DefaultContainerModule : IServiceManagerModule
    {
        public virtual string Name => "DefaultContainerModule";

        public int Priority => 0;
        
        public void Bootstrapp(IFramework framework)
        {
        }

        public IServiceManager CreateServiceManager()
        {
            var container = new ContainerWrapper(new ServiceCollection());
            container.RegisterInstance<IContainer>(container);
            return container;
        }
    }
}
