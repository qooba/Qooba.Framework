using System;

namespace Qooba.Framework.Abstractions
{
    internal class ServiceManager : IServiceBootstrapper, IServiceManager
    {
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

        public TService GetService<TService>() where TService : class => Manager.GetService<TService>();

        public IServiceManager AddService(Func<IServiceDescriptor, IServiceDescriptor> serviceDescriptorFactory) => Manager.AddService(serviceDescriptorFactory);

        public object GetService(Type serviceType) => Manager.GetService(serviceType);
    }
}