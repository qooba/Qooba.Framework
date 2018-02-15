using Microsoft.Extensions.Configuration;
using Qooba.Framework.Abstractions;
using Qooba.Framework.Configuration.Abstractions;

namespace Qooba.Framework.Configuration
{
    public class ConfigModule : IModule
    {
        public virtual string Name => "ConfigModule";

        public int Priority => 2;

        public void Bootstrapp(IFramework framework)
        {
            
        }
    }
}
