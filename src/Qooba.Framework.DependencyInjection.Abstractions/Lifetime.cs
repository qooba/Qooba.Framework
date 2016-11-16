namespace Qooba.Framework.DependencyInjection.Abstractions
{
    public enum Lifetime
    {
        Transistent,
        PerSession,
        PerRequest,
        PerThread,
        Singleton,
    }
}
