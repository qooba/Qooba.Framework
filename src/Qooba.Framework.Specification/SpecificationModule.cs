using Qooba.Framework.Abstractions;
using Qooba.Framework.Specification;
using Qooba.Framework.Specification.Abstractions;

namespace Qooba.Framework.Serialization
{
    public class SpecificationModule : IModule
    {
        public virtual string Name => "SpecificationModule";

        public int Priority => 10;

        public void Bootstrapp(IContainer container)
        {
            container.RegisterType<ISpecificationFactory, SpecificationFactory>();
            //ContainerManager.Current.RegisterType(typeof(IFetchStrategy<>), typeof(FetchStrategy<>));
        }
    }
}
