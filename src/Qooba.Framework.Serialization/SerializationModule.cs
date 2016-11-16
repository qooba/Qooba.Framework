using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.Serialization.Abstractions;
using System;

namespace Qooba.Framework.Serialization
{
    public class SerializationModule : IModule
    {
        public virtual string Name
        {
            get { return "SerializationModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<ISerializer, JsonSerializer>();
        }
    }
}
