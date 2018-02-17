using Qooba.Framework.Abstractions;

namespace Qooba.Framework
{
    public static class FrameworkLoggingExtensions
    {
        public static IFramework AddConsoleLogger(this IFramework framework)
        {
            return framework.AddService(s => s.Service<ILogTarget>().As(new ConsoleLogger()).Lifetime(Lifetime.Singleton));
        }

        public static IFramework AddTraceLogger(this IFramework framework)
        {
            return framework.AddService(s => s.Service<ILogTarget>().As(new TraceLogger()).Lifetime(Lifetime.Singleton));
        }
    }
}