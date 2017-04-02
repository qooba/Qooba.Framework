using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;

namespace Qooba.Framework.DependencyInjection.ContainerFactory
{
    public class ContainerFactoryModule: IModule
    {
        public string Name => "ContainerFactoryModule";
        
        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddTransientService<IFactory, Factory>();
            framework.AddTransientService(typeof(IFactory<>), typeof(Factory<>));
        }
    }
}
