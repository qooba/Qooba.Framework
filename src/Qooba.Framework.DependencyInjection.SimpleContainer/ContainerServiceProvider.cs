//using Qooba.Framework.DependencyInjection.Abstractions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Qooba.Framework.DependencyInjection.SimpleContainer
//{
//    public class ContainerServiceProvider : IServiceProvider
//    {
//        private readonly IContainer container;

//        public ContainerServiceProvider(IContainer container)
//        {
//            this.container = container;
//        }

//        public object GetService(Type serviceType)
//        {
//            var val =  this.container.Resolve(serviceType);
//            return val;
//        }
//    }
//}
