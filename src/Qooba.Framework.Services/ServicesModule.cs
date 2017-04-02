using Qooba.Framework.Abstractions;
using Qooba.Framework.Services.Abstractions;

namespace Qooba.Framework.Services
{
    public class ServicesModule : IModule
    {
        public virtual string Name => "ServicesModule";

        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddTransientService<IRestServiceFactory, RestServiceFactory>();
        }
    }
}
