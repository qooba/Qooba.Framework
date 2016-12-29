using Qooba.Framework.Abstractions;

namespace Qooba.Framework.DependencyInjection.AutofacContainer
{
    public class DefaultContainerModule : IModule
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
            //var container = new ContainerWrapper(new ServiceCollection());
            //container.RegisterInstance<IContainer>(container);
            //ContainerManager.SetContainer(container);
        }
    }
}
