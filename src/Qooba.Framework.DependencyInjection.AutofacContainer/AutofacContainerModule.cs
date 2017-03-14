using Autofac;
using Qooba.Framework.Abstractions;

namespace Qooba.Framework.DependencyInjection.AutofacContainer
{
    public class AutofacContainerModule : IContainerBootstrapper
    {
        public virtual string Name => "AutofacContainerModule";
        
        public int Priority => 0;
        
        public void Bootstrapp(Framework.Abstractions.IContainer container)
        {
        }

        public Framework.Abstractions.IContainer BootstrappContainer()
        {
            var container = new AutofacContainerWrapper(new ContainerBuilder());
            container.RegisterInstance<Framework.Abstractions.IContainer>(container);
            return container;
        }
    }
}
