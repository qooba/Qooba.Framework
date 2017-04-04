using Qooba.Framework.Serialization.Abstractions;
using Qooba.Framework.Serialization;
using Xunit;

namespace Qooba.Framework.Tests
{
    public class BootstrapperTests
    {
        [Fact]
        public void ttt()
        {
            Q.Create()
                .AddAssembly(a => a.All())
                .AddModule(m => m.Module(new SerializationModule()))
                .AddService(s => s.Service<ISerializer>().As<JsonSerializer>())
                .Bootstrapp()
                .GetService<ISerializer>();
        }
    }
}