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

        public Framework.Abstractions.IContainer BootstrappContainer() => new AutofacContainerWrapper(new ContainerBuilder());
    }
}
