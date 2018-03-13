using Qooba.Framework.Abstractions;

namespace Qooba.Framework.Serialization
{
    public class SerializationModule : IModule
    {
        public virtual string Name => "SerializationModule";

        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddTransientService<ISerializer, JsonSerializer>();
        }
    }
}
