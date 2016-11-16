using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.Specification.Abstractions;
using System;

namespace Qooba.Framework.UnitOfWork.EntityFramework
{
    public class UnitOfWorkModule : IModule
    {
        public virtual string Name
        {
            get { return "UnitOfWorkModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType(typeof(UnitOfWork<>));
            ContainerManager.Current.RegisterType(typeof(IFetchStrategy<>), typeof(EFFetchStrategy<>));
        }
    }
}
