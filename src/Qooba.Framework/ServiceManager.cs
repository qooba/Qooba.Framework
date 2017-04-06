using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Qooba.Framework.Abstractions
{
    internal class ServiceManager : IServiceBootstrapper, IServiceManager
    {
        private static IDictionary<Func<IServiceDescriptor, IServiceDescriptor>,bool> Services = new ConcurrentDictionary<Func<IServiceDescriptor, IServiceDescriptor>,bool>();

        private static IServiceManager manager;

        internal static IServiceManager Manager
        {
            get { return manager; }
            set { manager = value; }
        }

        public void SetServiceManager(IServiceManager serviceManager)
        {
            Manager = serviceManager;
        }

        public IEnumerable<Func<IServiceDescriptor, IServiceDescriptor>> GetServices() => Services.ToList().Where(x=>x.Value == false).Select(x=>x.Key);

        public TService GetService<TService>() where TService : class => Manager.GetService<TService>();

        public IServiceManager AddService(Func<IServiceDescriptor, IServiceDescriptor> serviceDescriptorFactory)
        {
            var hasManager = Manager != null;
            Services[serviceDescriptorFactory] = hasManager;
            if (hasManager)
            {
                return Manager.AddService(serviceDescriptorFactory);
            }

            return this;
        }

        public object GetService(Type serviceType) => Manager.GetService(serviceType);

        public TService GetService<TService>(object key) where TService : class => Manager.GetService<TService>(key);

        public object GetService(object key, Type serviceType) => Manager.GetService(key, serviceType);

        public IEnumerable<TService> GetServices<TService>() where TService : class => Manager.GetServices<TService>();

        public IEnumerable<object> GetServices(Type serviceType) => Manager.GetServices(serviceType);
    }
}