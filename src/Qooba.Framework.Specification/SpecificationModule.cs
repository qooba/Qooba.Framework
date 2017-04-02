using Qooba.Framework.Abstractions;
using Qooba.Framework.Specification;
using Qooba.Framework.Specification.Abstractions;

namespace Qooba.Framework.Serialization
{
    public class SpecificationModule : IModule
    {
        public virtual string Name => "SpecificationModule";

        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddTransientService<ISpecificationFactory, SpecificationFactory>();
            //framework.AddTransientService(typeof(IFetchStrategy<>), typeof(FetchStrategy<>));
        }
    }
}
