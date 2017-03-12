using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;

namespace Qooba.Framework.DependencyInjection.ContainerFactory
{
    public class ContainerFactoryModule: IModule
    {
        public string Name => "ContainerFactoryModule";
        
        public int Priority => 10;

        public void Bootstrapp(IContainer container)
        {
            container.RegisterType<IFactory, Factory>();
            container.RegisterType(typeof(IFactory<>), typeof(Factory<>));
        }
    }
}
