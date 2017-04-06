using Qooba.Framework.Serialization.Abstractions;
using Qooba.Framework.Serialization;
using Xunit;
using Qooba.Framework.DependencyInjection.SimpleContainer;

namespace Qooba.Framework.Tests
{
    public class BootstrapperTests
    {
        [Fact]
        public void AddAllAssembliesTest()
        {
            var serializer = FrameworkBuilder.Create()
                .AddAssembly(a => a.All())
                .Bootstrapp()
                .GetService<ISerializer>();

            Assert.True(serializer is JsonSerializer);
        }

        [Fact]
        public void AddModulesTest()
        {
            var serializer = FrameworkBuilder.Create()
                .AddModule(m => m.Module(new SimpleContainerModule()))
                .AddModule(m => m.Module(new SerializationModule()))
                .Bootstrapp()
                .GetService<ISerializer>();

            Assert.True(serializer is JsonSerializer);
        }

        [Fact]
        public void AddServiceTest()
        {
            var serializer = FrameworkBuilder.Create()
                .AddModule(m => m.Module(new SimpleContainerModule()))
                .AddService(s => s.Service<ISerializer>().As<JsonSerializer>())
                .Bootstrapp()
                .GetService<ISerializer>();

            Assert.True(serializer is JsonSerializer);
        }
    }
}