namespace Qooba.Framework.Abstractions
{
    public interface IServiceProvider : System.IServiceProvider
    {
        TService GetService<TService>() where TService : class;
    }
}
