using Qooba.Framework.Abstractions;
using Qooba.Framework.Cqrs.Abstractions;

namespace Qooba.Framework.Cqrs
{
    public class CqrsModule : IModule
    {
        public virtual string Name => "CqrsModule";

        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddTransientService<ICommandDispatcher, CommandDispatcher>();
            framework.AddTransientService<IQueryDispatcher, QueryDispatcher>();
            framework.AddTransientService<ISpecificationQueryDispatcher, SpecificationQueryDispatcher>();
        }
    }
}
