using Xunit;

namespace Qooba.Framework.Tests
{
    public class BootstrapperTests
    {
        // [Fact]
        // public void AddAllAssembliesTest()
        // {
        //     var serializer = FrameworkBuilder.Create()
        //         .AddAssembly(a => a.All())
        //         .Bootstrapp()
        //         .GetService<ISerializer>();

        //     Assert.True(serializer is JsonSerializer);
        // }

        // [Fact]
        // public void AddModulesTest()
        // {
        //     var serializer = FrameworkBuilder.Create()
        //         .AddModule(m => m.Module(new SimpleContainerModule()))
        //         .AddModule(m => m.Module(new SerializationModule()))
        //         .Bootstrapp()
        //         .GetService<ISerializer>();

        //     Assert.True(serializer is JsonSerializer);
        // }

        [Fact]
        public void AddServiceTest()
        {
            var test = FrameworkBuilder.Create()
                .AddService(s => s.Service<ITest>().As<Test>())
                .Bootstrapp()
                .GetService<ITest>();

            Assert.True(test is Test);
        }

        public interface ITest
        {
            string Run();
        }

        public class Test : ITest
        {
            public string Run() => "hello";
        }

    }
}