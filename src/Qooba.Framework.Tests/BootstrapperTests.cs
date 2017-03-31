using Qooba.Framework.Abstractions;
using Qooba.Framework.Serialization.Abstractions;
using Qooba.Framework.Serialization;
using Qooba.Framework.DependencyInjection.SimpleContainer;
using System;
using Xunit;

namespace Qooba.Framework.Tests
{
    public class BootstrapperTests
    {
        IBootstrapper bootstrapper;

        public BootstrapperTests()
        {
            this.bootstrapper = Bootstrapper.Instance;
        }

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

        [Fact]
        public void BootstrappModulesTest()
        {
            var serializer = this.bootstrapper.BootstrappModules(new SimpleContainerModule(), new SerializationModule()).Container.Resolve<ISerializer>();
            var i = new TestClass { Test = "value" };

            var o = serializer.Serialize(i);
            var ii = serializer.Deserialize<TestClass>(o);

            Assert.Equal(i.Test, ii.Test);
        }

        [Fact]
        public void BootstrappTest()
        {
            var serializer = this.bootstrapper.Bootstrapp().Container.Resolve<ISerializer>();
            var i = new TestClass { Test = "value" };

            var o = serializer.Serialize(i);
            var ii = serializer.Deserialize<TestClass>(o);

            Assert.Equal(i.Test, ii.Test);
        }
    }

    public class TestClass
    {
        public string Test { get; set; }
    }
}
