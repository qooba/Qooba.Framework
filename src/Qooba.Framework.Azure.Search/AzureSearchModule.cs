using Qooba.Framework.Abstractions;
using Qooba.Framework.Azure.Search;
using Qooba.Framework.Azure.Search.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;

namespace Qooba.Framework.Azure.IoT
{
    public class AzureSearchModule : IModule
    {
        public virtual string Name
        {
            get { return "AzureSearchModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<IAzureSearch, AzureSearch>();
        }
    }
}
