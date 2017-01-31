using Autofac;
using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;

namespace Qooba.Framework.DependencyInjection.AutofacContainer
{
    public class AutofacContainerModule : IModule
    {
        public virtual string Name
        {
            get { return "AutofacContainerModule"; }
        }
        
        public int Priority
        {
            get { return 0; }
        }

        public void Bootstrapp()
        {
            var container = new AutofacContainerWrapper(new ContainerBuilder());
            ContainerManager.SetContainer(container);
        }
    }
}
