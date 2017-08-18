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
            container.RegisterType(null, typeof(IService), typeof(SomeService), Framework.Abstractions.Lifetime.Transistent);

            var o = container.IsRegistered(typeof(IService), null);

            Assert.True(o);
        }

        [Fact]
        public void IsRegisteredKeyedTest()
        {
            var key = "key";
            var container = new Container();
            container.RegisterType(key, typeof(IService), typeof(SomeService), Framework.Abstractions.Lifetime.Transistent);

            var o = container.IsRegistered(typeof(IService), key);

            Assert.True(o);
        }

        [Fact]
        public void RegisterTypeTest()
        {
            var container = new Container();
            container.RegisterType(null, typeof(IService), typeof(SomeService), Framework.Abstractions.Lifetime.Transistent);
            container.RegisterType(null, typeof(IClient), typeof(SomeClient), Framework.Abstractions.Lifetime.Transistent);

            var o = container.Resolve(null, typeof(IClient));

            Assert.True(o is IClient);
        }

        [Fact]
        public void RegisterTypeKeyedTest()
        {
            var key = "key";
            var container = new Container();
            container.RegisterType(null, typeof(IService), typeof(SomeService), Framework.Abstractions.Lifetime.Transistent);
            container.RegisterType(key, typeof(IClient), typeof(SomeClient), Framework.Abstractions.Lifetime.Transistent);

            var o = container.Resolve(key, typeof(IClient));

            Assert.True(o is IClient);
        }

        [Fact]
        public void RegisterSingletonTypeKeyedTest()
        {
            var key = "key";
            var key2 = "key2";
            var container = new Container();
            container.RegisterType(null, typeof(IService), typeof(SomeService), Framework.Abstractions.Lifetime.Singleton);
            container.RegisterType(key, typeof(IClient), typeof(SomeClient), Framework.Abstractions.Lifetime.Singleton);
            container.RegisterType(key2, typeof(IClient), typeof(SomeClient2), Framework.Abstractions.Lifetime.Singleton);

            var o = container.Resolve(key, typeof(IClient));
            var o2 = container.Resolve(key2, typeof(IClient));

            Assert.True(o is SomeClient);
            Assert.True(o2 is SomeClient2);
        }

        [Fact]
        public void RegisterGenericTest()
        {
            var container = new Container();
            container.RegisterType(null, typeof(IService), typeof(SomeService), Framework.Abstractions.Lifetime.Transistent);
            container.RegisterType(null, typeof(IClient), typeof(SomeClient), Framework.Abstractions.Lifetime.Transistent);
            container.RegisterType(null, typeof(IRepository<>), typeof(Repository<>), Framework.Abstractions.Lifetime.Transistent);

            var o = container.Resolve(null, typeof(IRepository<IClient>)) as IRepository<IClient>;

            Assert.True(o.Create() is IClient);
        }

        [Fact]
        public void ResolveEnumerablesTest()
        {
            var container = new Container();
            container.RegisterType(null, typeof(IService), typeof(SomeService), Framework.Abstractions.Lifetime.Transistent);
            container.RegisterType(null, typeof(IService), typeof(SomeService2), Framework.Abstractions.Lifetime.Transistent);
            container.RegisterType(null, typeof(IClient), typeof(SomeClient2), Framework.Abstractions.Lifetime.Transistent);

            var o = container.Resolve(null, typeof(IClient)) as IClient;

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
