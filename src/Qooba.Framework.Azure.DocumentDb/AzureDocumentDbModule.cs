using Microsoft.Azure.Documents.Client;
using Qooba.Framework.Abstractions;
using Qooba.Framework.Azure.DocumentDb.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;

namespace Qooba.Framework.Azure.IoT
{
    public class AzureDocumentDbModule : IModule
    {
        public virtual string Name
        {
            get { return "AzureDocumentDbModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType(typeof(IDocumentDbRepository<>), typeof(DocumentDbRepository<>));
        }
    }
}
