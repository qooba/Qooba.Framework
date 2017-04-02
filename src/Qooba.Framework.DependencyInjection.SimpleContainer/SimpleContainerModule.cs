using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;

namespace Qooba.Framework.DependencyInjection.SimpleContainer
{
    public class SimpleContainerModule : IServiceManagerModule
    {
        public virtual string Name => "SimpleContainerModule";

        public int Priority => 0;
        
        public void Bootstrapp(IFramework framework)
        {
        }

        public IServiceManager CreateServiceManager()
        {
            var container = new Container();
            container.RegisterInstance<IContainer>(container);
            return container;
        }
    }
}
