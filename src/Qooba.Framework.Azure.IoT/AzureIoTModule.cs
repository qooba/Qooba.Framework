using Qooba.Framework.Abstractions;
using Qooba.Framework.Azure.IoT.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;

namespace Qooba.Framework.Azure.IoT
{
    public class AzureIoTModule : IModule
    {
        public virtual string Name
        {
            get { return "AzureIoTModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<IIoTHub, IoTHub>();
        }
    }
}
