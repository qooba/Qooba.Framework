using Qooba.Framework.Abstractions;
using Qooba.Framework.Specification.Abstractions;

namespace Qooba.Framework.UnitOfWork.EntityFramework
{
    public class UnitOfWorkModule : IModule
    {
        public virtual string Name => "UnitOfWorkModule";

        public int Priority => 10;

        public void Bootstrapp(IContainer container)
        {
            container.RegisterType(typeof(UnitOfWork<>));
            container.RegisterType(typeof(IFetchStrategy<>), typeof(EFFetchStrategy<>));
        }
    }
}
