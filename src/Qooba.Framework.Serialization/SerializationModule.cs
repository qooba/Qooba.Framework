using Qooba.Framework.Abstractions;
using Qooba.Framework.Serialization.Abstractions;

namespace Qooba.Framework.Serialization
{
    public class SerializationModule : IModule
    {
        public virtual string Name => "SerializationModule";

        public int Priority => 10;

        public void Bootstrapp(IContainer container)
        {
            container.RegisterType<ISerializer, JsonSerializer>();
        }
    }
}
