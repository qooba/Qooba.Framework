using Qooba.Framework.Abstractions;

namespace Qooba.Framework.ServiceFabric
{
    public class ServiceFabricModule : IModule
    {
        public virtual string Name => "ServiceFabricModule"; 

        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            //ContainerManager.Current.RegisterType<ISerializer, JsonSerializer>();
        }
    }
}
