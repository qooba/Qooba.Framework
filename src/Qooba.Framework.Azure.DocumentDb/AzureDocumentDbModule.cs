using Qooba.Framework.Abstractions;
using Qooba.Framework.Azure.DocumentDb;
using Qooba.Framework.Azure.DocumentDb.Abstractions;

namespace Qooba.Framework.Azure.IoT
{
    public class AzureDocumentDbModule : IModule
    {
        public virtual string Name => "AzureDocumentDbModule";

        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddSingletonService<IDocumentDbConfig, DocumentDbConfig>();
            framework.AddTransientService(typeof(IDocumentDbRepository<>), typeof(DocumentDbRepository<>));
        }
    }
}
