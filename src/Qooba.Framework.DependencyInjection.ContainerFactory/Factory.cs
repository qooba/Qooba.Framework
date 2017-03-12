using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Qooba.Framework.DependencyInjection.ContainerFactory
{
    public class Factory<T> : IFactory<T>
        where T : class
    {
        private readonly IContainer container;

        public Factory(IContainer container)
        {
            this.container = container;
        }

        public T Create() => this.container.Resolve<T>();
        
        public T Create(string key) => this.container.Resolve<T>(key);
        
        public IList<T> CreateAll() => this.container.ResolveAll<T>().ToList();
    }

    public class Factory : IFactory
    {
        private readonly IContainer container;

        public Factory(IContainer container)
        {
            this.container = container;
        }

        public T Create<T>() where T : class => this.container.Resolve<T>();
        
        public T Create<T>(string key) where T : class => this.container.Resolve<T>(key);
        
        public IList<T> CreateAll<T>() where T : class => this.container.ResolveAll<T>().ToList();
    }
}
