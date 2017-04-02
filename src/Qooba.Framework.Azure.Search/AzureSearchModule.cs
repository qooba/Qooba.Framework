using Qooba.Framework.Abstractions;
using Qooba.Framework.Azure.Search;
using Qooba.Framework.Azure.Search.Abstractions;

namespace Qooba.Framework.Azure.IoT
{
    public class AzureSearchModule : IModule
    {
        public virtual string Name => "AzureSearchModule";


        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddSingletonService<IAzureSearchConfig, AzureSearchConfig>();
            framework.AddTransientService<IAzureSearch, AzureSearch>();
        }
    }
}
