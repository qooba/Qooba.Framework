namespace Qooba.Framework.Abstractions
{
    public interface IConfiguration
    {
        string this[string key] { get; }
    }
}
