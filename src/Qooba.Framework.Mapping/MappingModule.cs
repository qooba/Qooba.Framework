using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.Mapping.Abstractions;
using System;

namespace Qooba.Framework.Mapping
{
    public class MappingModule : IModule
    {
        public virtual string Name
        {
            get { return "MappingModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<IMapper, QMap>();
        }
    }
}
