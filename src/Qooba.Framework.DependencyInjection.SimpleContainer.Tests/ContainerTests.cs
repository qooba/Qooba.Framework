using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Qooba.Framework.DependencyInjection.SimpleContainer.Tests
{
    public class ContainerTests
    {
        [Fact]
        public void IsRegisteredTest()
        {
            var container = new Container();
            container.RegisterType<IService, SomeService>();

            var o = container.IsRegistered<IService>();

            Assert.True(o);
        }

        [Fact]
        public void IsRegisteredKeyedTest()
        {
            var key = "key";
            var container = new Container();
            container.RegisterType<IService, SomeService>(key);

            var o = container.IsRegistered<IService>(key);

            Assert.True(o);
        }

        [Fact]
        public void RegisterTypeTest()
        {
            var container = new Container();
            container.RegisterType<IService, SomeService>();
            container.RegisterType<IClient, SomeClient>();

            var o = container.Resolve<IClient>();

            Assert.True(o is IClient);
        }

        [Fact]
        public void RegisterTypeKeyedTest()
        {
            var key = "key";
            var container = new Container();
            container.RegisterType<IService, SomeService>();
            container.RegisterType<IClient, SomeClient>(key);

            var o = container.Resolve<IClient>(key);

            Assert.True(o is IClient);
        }

        [Fact]
        public void RegisterGenericTest()
        {
            var container = new Container();
            container.RegisterType<IService, SomeService>();
            container.RegisterType<IClient, SomeClient>();
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));

            var o = container.Resolve<IRepository<IClient>>();

            Assert.True(o.Create() is IClient);
        }

        [Fact]
        public void ResolveEnumerablesTest()
        {
            var container = new Container();
            container.RegisterType<IService, SomeService>();
            container.RegisterType<IService, SomeService2>();
            container.RegisterType<IClient, SomeClient2>();
            
            var o = container.Resolve<IClient>();

            Assert.True(o.Service is IService);
        }
    }
    
    public interface IService { }
    public class SomeService : IService { }
    public class SomeService2 : IService { }

    public interface IClient { IService Service { get; } }
    public class SomeClient : IClient
    {
        public IService Service { get; private set; }
        public SomeClient(IService service) { Service = service; }
    }

    public class SomeClient2 : IClient
    {
        public IService Service => Services.Last();
        public IList<IService> Services { get; private set; }
        public SomeClient2(IEnumerable<IService> services) { Services = services.ToList(); }
    }

    public interface IRepository<T>
    {
        T Create();
    }

    public class Repository<T> : IRepository<T>
    {
        private readonly T t;
        public Repository(T t)
        {
            this.t = t;
        }

        public T Create()
        {
            return t;
        }
    }
}
