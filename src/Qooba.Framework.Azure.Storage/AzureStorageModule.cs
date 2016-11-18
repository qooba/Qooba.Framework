using Qooba.Framework.Abstractions;
using Qooba.Framework.Azure.Storage.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;
using System;

namespace Qooba.Framework.Azure.Storage
{
    public class AzureStorageModule : IModule
    {
        public virtual string Name
        {
            get { return "AzureStorageModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<IAzureBlob, AzureBlob>();
            ContainerManager.Current.RegisterType<IAzureBlobQueue, AzureBlobQueue>();
            ContainerManager.Current.RegisterType<IAzureBlobTable, AzureBlobTable>();
            ContainerManager.Current.RegisterType(typeof(IAzureBlobTableRepository<>), typeof(AzureBlobTableRepository<>));
            ContainerManager.Current.RegisterType(typeof(IRepositoryCommands<>), typeof(AzureBlobTableRepository<>));
            ContainerManager.Current.RegisterType(typeof(IRepositoryQueries<>), typeof(AzureBlobTableRepository<>));
        }
    }
}
