using System;

namespace Qooba.Framework.Abstractions
{
    internal class ServiceManager : IServiceBootstrapper, IServiceManager
    {
        private static IServiceManager serviceManager;

        public static IServiceManager ServiceManager
        {
            get { return serviceManager; }
            set { serviceManager = value; }
        }

        private static Lazy<ServiceManager> current = new Lazy<ServiceManager>(() => new ServiceManager());

        public static ServiceManager Current => current.Value;

        public void SetServiceManager(IServiceManager serviceManager)
        {
            ServiceManager = serviceManager;
        }

        public TService GetService<TService>() where TService : class => ServiceManager.GetService<TService>();

        public IServiceManager AddService<TFrom, TTo>() where TTo : class, TFrom where TFrom : class => ServiceManager.AddService<TFrom, TTo>();

        public IServiceManager AddService<TFrom, TTo>(TTo service) where TTo : class, TFrom where TFrom : class => ServiceManager.AddService<TFrom, TTo>(service);
    }
}
