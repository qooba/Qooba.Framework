using Qooba.Framework.Abstractions;
using Qooba.Framework.Logging.Abstractions;

namespace Qooba.Framework.Logging.Console
{
    public class ConsoleModule : IModule
    {
        public virtual string Name => "ConsoleModule";

        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddTransientService<ILogger, ConsoleLogger>();
        }
    }
}
