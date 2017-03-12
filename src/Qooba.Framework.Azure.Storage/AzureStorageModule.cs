using Qooba.Framework.Abstractions;
using Qooba.Framework.Azure.Storage.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;

namespace Qooba.Framework.Azure.Storage
{
    public class AzureStorageModule : IModule
    {
        public virtual string Name =>"AzureStorageModule";

        public int Priority => 10;

        public void Bootstrapp(IContainer container)
        {
            container.RegisterType<IAzureBlob, AzureBlob>();
            container.RegisterType<IAzureBlobQueue, AzureBlobQueue>();
            container.RegisterType<IAzureBlobTable, AzureBlobTable>();
            container.RegisterType(typeof(IAzureBlobTableRepository<>), typeof(AzureBlobTableRepository<>));
            container.RegisterType(typeof(IRepositoryCommands<>), typeof(AzureBlobTableRepository<>));
            container.RegisterType(typeof(IRepositoryQueries<>), typeof(AzureBlobTableRepository<>));
        }
    }
}
