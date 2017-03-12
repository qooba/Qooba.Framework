using Qooba.Framework.Abstractions;
using Qooba.Framework.Cqrs.Abstractions;

namespace Qooba.Framework.Cqrs
{
    public class CqrsModule : IModule
    {
        public virtual string Name => "CqrsModule";

        public int Priority => 10;

        public void Bootstrapp(IContainer container)
        {
            container.RegisterType<ICommandDispatcher, CommandDispatcher>();
            container.RegisterType<IQueryDispatcher, QueryDispatcher>();
            container.RegisterType<ISpecificationQueryDispatcher, SpecificationQueryDispatcher>();
        }
    }
}
