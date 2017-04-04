using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;

namespace Qooba.Framework.DependencyInjection.DefaultContainer
{
    public class ContainerWrapper : IContainer
    {
        private readonly IServiceCollection services;
        public ContainerWrapper(IServiceCollection services)
        {
            this.services = services;
        }

        public bool IsRegistered(Type typeToCheck, object keyToCheck)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterInstance(object key, Type from, object instance)
        {
            this.services.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(from, instance));
            return this;
        }

        public IContainer RegisterType(object key, Type from, Type to, Lifetime lifetime)
        {
            this.services.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(from, to, Enum.TryParse(lifetime.ToString(), out ServiceLifetime lt) ? lt : ServiceLifetime.Transient));
            return this;
        }

        public IContainer RegisterFactory(object key, Type from, Func<IContainer, object> implementationFactory, Lifetime lifetime)
        {
            Func<System.IServiceProvider, object> factory = sp => implementationFactory(sp.GetService(typeof(IContainer)) as IContainer);
            this.services.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(from, factory, Enum.TryParse(lifetime.ToString(), out ServiceLifetime lt) ? lt : ServiceLifetime.Transient));
            return this;
        }

        public object Resolve(object key, Type from) => this.services.BuildServiceProvider().GetService(from);

        public IEnumerable<object> ResolveAll(Type from) => this.services.BuildServiceProvider().GetServices(from);

        public void Populate(object services)
        {
            var servicesCollection = services as IServiceCollection;
            if (servicesCollection != null)
            {
                foreach (var service in servicesCollection)
                {
                    this.services.Add(service);
                }
            }
        }

        public IServiceManager AddService(Func<IServiceDescriptor, IServiceDescriptor> serviceDescriptorFactory)
        {
            Abstractions.ServiceDescriptor serviceDescriptor = (Abstractions.ServiceDescriptor)serviceDescriptorFactory(new Abstractions.ServiceDescriptor());

            if (serviceDescriptor.ServiceType == null)
            {
                throw new InvalidOperationException("Upps ... service type not defined.");
            }

            if (serviceDescriptor.ImplementationInstance != null)
            {
                this.RegisterInstance(serviceDescriptor.Key, serviceDescriptor.ServiceType, serviceDescriptor.ImplementationInstance);
            }
            else if (serviceDescriptor.ImplementationType != null)
            {
                this.RegisterType(serviceDescriptor.Key, serviceDescriptor.ServiceType, serviceDescriptor.ImplementationType, serviceDescriptor.LifetimeType);
            }
            else if (serviceDescriptor.ImplementationFactory != null)
            {
                this.RegisterFactory(serviceDescriptor.Key, serviceDescriptor.ServiceType, serviceDescriptor.ImplementationFactory, serviceDescriptor.LifetimeType);
            }
            else
            {
                throw new InvalidOperationException("Upps ... implementation type not defined.");
            }

            return this;
        }

        public TService GetService<TService>() where TService : class => GetService(typeof(TService)) as TService;

        public object GetService(Type serviceType) => this.Resolve(null, serviceType);

        public TService GetService<TService>(object key) where TService : class => this.Resolve(key, typeof(TService)) as TService;

        public object GetService(object key, Type serviceType) => this.Resolve(key, serviceType);
    }
}