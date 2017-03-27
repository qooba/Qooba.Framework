using Qooba.Framework.Abstractions;
using Qooba.Framework.Azure.IoT.Abstractions;

namespace Qooba.Framework.Azure.IoT
{
    public class AzureIoTModule : IModule
    {
        public virtual string Name => "AzureIoTModule";

        public int Priority => 10;

        public void Bootstrapp(IContainer container)
        {
            container.RegisterType<IIoTHubConfig, IoTHubConfig>(Lifetime.Singleton);
            container.RegisterType<IIoTHub, IoTHub>();
        }
    }
}
