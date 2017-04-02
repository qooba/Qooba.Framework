using Qooba.Framework.Abstractions;
using Qooba.Framework.Azure.Storage.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;

namespace Qooba.Framework.Azure.Storage
{
    public class AzureStorageModule : IModule
    {
        public virtual string Name => "AzureStorageModule";

        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddSingletonService<IAzureStorageConfig, AzureStorageConfig>();
            framework.AddTransientService<IAzureBlob, AzureBlob>();
            framework.AddTransientService<IAzureBlobQueue, AzureBlobQueue>();
            framework.AddTransientService<IAzureBlobTable, AzureBlobTable>();
            framework.AddTransientService(typeof(IAzureBlobTableRepository<>), typeof(AzureBlobTableRepository<>));
            framework.AddTransientService(typeof(IRepositoryCommands<>), typeof(AzureBlobTableRepository<>));
            framework.AddTransientService(typeof(IRepositoryQueries<>), typeof(AzureBlobTableRepository<>));
        }
    }
}
