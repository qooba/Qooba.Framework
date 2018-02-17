using Qooba.Framework.Abstractions;

namespace Qooba.Framework
{
    public class Configuration : IConfiguration
    {
        public string this[string key] => System.Environment.GetEnvironmentVariable(key);
    }
}