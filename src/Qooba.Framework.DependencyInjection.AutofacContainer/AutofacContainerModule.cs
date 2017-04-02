using Autofac;
using Qooba.Framework.Abstractions;

namespace Qooba.Framework.DependencyInjection.AutofacContainer
{
    public class AutofacContainerModule : IServiceManagerModule
    {
        public virtual string Name => "AutofacContainerModule";
        
        public int Priority => 0;
        
        public void Bootstrapp(IFramework framework)
        {
        }

        public IServiceManager CreateServiceManager()
        {
            var container = new AutofacContainerWrapper(new ContainerBuilder());
            container.RegisterInstance<Abstractions.IContainer>(container);
            return container;
        }
    }
}
