namespace Qooba.Framework.Configuration.Abstractions
{
    public interface IConfig
    {
        string this[string key] { get; }
    }
}
