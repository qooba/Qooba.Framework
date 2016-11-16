using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.Specification;
using Qooba.Framework.Specification.Abstractions;
using System;

namespace Qooba.Framework.Serialization
{
    public class SpecificationModule : IModule
    {
        public virtual string Name
        {
            get { return "SpecificationModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<ISpecificationFactory, SpecificationFactory>();
            //ContainerManager.Current.RegisterType(typeof(IFetchStrategy<>), typeof(FetchStrategy<>));
        }
    }
}
