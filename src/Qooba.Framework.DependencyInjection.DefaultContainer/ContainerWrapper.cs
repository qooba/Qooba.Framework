using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;

namespace Qooba.Framework.DependencyInjection.DefaultContainer
{
    public class ContainerWrapper : BaseContainer
    {
        private readonly IServiceCollection services;
        public ContainerWrapper(IServiceCollection services)
        {
            this.services = services;
        }

        public override bool IsRegistered(Type typeToCheck, object keyToCheck)
        {
            throw new NotImplementedException();
        }

        public override IContainer RegisterInstance(object key, Type from, object instance)
        {
            this.services.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(from, instance));
            return this;
        }

        public override IContainer RegisterType(object key, Type from, Type to, Lifetime lifetime)
        {
            this.services.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(from, to, Enum.TryParse(lifetime.ToString(), out ServiceLifetime lt) ? lt : ServiceLifetime.Transient));
            return this;
        }

        public override IContainer RegisterFactory(object key, Type from, Func<IContainer, object> implementationFactory, Lifetime lifetime)
        {
            Func<System.IServiceProvider, object> factory = sp => implementationFactory(sp.GetService(typeof(IContainer)) as IContainer);
            this.services.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(from, factory, Enum.TryParse(lifetime.ToString(), out ServiceLifetime lt) ? lt : ServiceLifetime.Transient));
            return this;
        }

        public override object Resolve(object key, Type from) => this.services.BuildServiceProvider().GetService(from);

        public override IEnumerable<object> ResolveAll(Type from) => this.services.BuildServiceProvider().GetServices(from);

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
    }
}