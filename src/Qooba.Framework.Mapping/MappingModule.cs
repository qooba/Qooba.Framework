using Qooba.Framework.Abstractions;
using Qooba.Framework.Mapping.Abstractions;

namespace Qooba.Framework.Mapping
{
    public class MappingModule : IModule
    {
        public virtual string Name => "MappingModule";

        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddTransientService<IMapper, QMap>();
        }
    }
}
