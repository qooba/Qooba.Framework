using Qooba.Framework.Abstractions;
using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;

namespace Qooba.Framework.Cqrs
{
    public class CqrsModule : IModule
    {
        public virtual string Name
        {
            get { return "CqrsModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<ICommandDispatcher, CommandDispatcher>();
            ContainerManager.Current.RegisterType<IQueryDispatcher, QueryDispatcher>();
            ContainerManager.Current.RegisterType<ISpecificationQueryDispatcher, SpecificationQueryDispatcher>();
        }
    }
}
