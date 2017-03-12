namespace Qooba.Framework.Abstractions
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
