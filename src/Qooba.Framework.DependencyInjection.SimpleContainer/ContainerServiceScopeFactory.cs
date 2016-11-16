//using Microsoft.Extensions.DependencyInjection;
//using Qooba.Framework.DependencyInjection.Abstractions;

//namespace Qooba.Framework.DependencyInjection.SimpleContainer
//{
//    public class ContainerServiceScopeFactory : IServiceScopeFactory
//    {
//        private readonly IContainer container;

//        public ContainerServiceScopeFactory(IContainer container)
//        {
//            this.container = container;
//        }

//        public IServiceScope CreateScope()
//        {
//            return new ContainerServiceScope(this.container);
//        }
//    }
//}
