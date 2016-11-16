using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;
using System.Reflection;

namespace Qooba.Framework.DependencyInjection.ContainerFactory
{
    public class ContainerFactoryModule: IModule
    {
        public string Name
        {
            get { return "ContainerFactoryModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<IFactory, Factory>();
            ContainerManager.Current.RegisterType(typeof(IFactory<>), typeof(Factory<>));
        }
    }
}
