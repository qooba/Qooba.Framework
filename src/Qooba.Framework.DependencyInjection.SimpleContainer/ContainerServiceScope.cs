//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using Qooba.Framework.DependencyInjection.Abstractions;

//namespace Qooba.Framework.DependencyInjection.SimpleContainer
//{
//    public class ContainerServiceScope : IServiceScope
//    {
//        private readonly IContainer container;
        
//        public ContainerServiceScope(IContainer container)
//        {
//            this.container = container;
//            this.ServiceProvider = container.Resolve<IServiceProvider>();
//        }

//        public IServiceProvider ServiceProvider { get; private set; }
        
//        public void Dispose()
//        {
//            //TODO: 
//        }
//    }
//}
