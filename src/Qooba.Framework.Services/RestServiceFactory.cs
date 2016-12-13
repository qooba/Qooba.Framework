using System;
using Microsoft.Rest;
using Qooba.Framework.Services.Abstractions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Qooba.Framework.Mapping.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Services.Abstractions.Models;

namespace Qooba.Framework.Services
{
    public class RestServiceFactory : IRestServiceFactory
    {
        private static IDictionary<Type, object> servicesFactory = new ConcurrentDictionary<Type, object>();

        private readonly IRoutingManager routingManager;

        private readonly IHeaderManager headerManager;

        private readonly IMapper mapper;

        public RestServiceFactory(IRoutingManager routingManager, IMapper mapper, IHeaderManager headerManager)
        {
            this.routingManager = routingManager;
            this.headerManager = headerManager;
            this.mapper = mapper;
        }

        public void Register<TServiceInterface, TService>(Func<Uri, TService> serviceFactory)
            where TService : ServiceClient<TService>, TServiceInterface
            where TServiceInterface : IDisposable
        {
            var serviceType = typeof(TServiceInterface);
            Func<TService> func = () =>
            {
                var uri = this.routingManager.GetServiceUri(serviceType.FullName);
                var service = serviceFactory(uri);
                PrepareHeader(serviceType, service);
                return service;
            };

            servicesFactory[serviceType] = new Lazy<TServiceInterface>(() => func());
        }

        public async Task<TResponse> Invoke<TServiceInterface, TResponse>(Func<TServiceInterface, Task<TResponse>> action)
            where TServiceInterface : IDisposable
        {
            var serviceType = typeof(TServiceInterface);
            var serviceFactory = servicesFactory[serviceType] as Lazy<TServiceInterface>;
            var service = serviceFactory.Value;
            return await action(service);
        }

        public async Task<TOutput> Invoke<TServiceInterface, TInput, TRequest, TResponse, TOutput>(Func<TServiceInterface, TRequest, Task<TResponse>> action, TInput input)
            where TServiceInterface : IDisposable
            where TOutput : BaseOutput
        {
            var serviceType = typeof(TServiceInterface);
            var serviceFactory = servicesFactory[serviceType] as Lazy<TServiceInterface>;
            var service = serviceFactory.Value;
            var request = this.mapper.Map<TInput, TRequest>(input);
            var response = await action(service, request);
            return this.mapper.Map<TResponse, TOutput>(response);
        }

        public async Task<TOutput> Invoke<TServiceInterface, TResponse, TOutput>(Func<TServiceInterface, Task<TResponse>> action)
            where TServiceInterface : IDisposable
            where TOutput : BaseOutput
        {
            var serviceType = typeof(TServiceInterface);
            var serviceFactory = servicesFactory[serviceType] as Lazy<TServiceInterface>;
            var service = serviceFactory.Value;
            var response = await action(service);
            return this.mapper.Map<TResponse, TOutput>(response);
        }

        private void PrepareHeader<TService>(Type serviceType, TService service)
            where TService : ServiceClient<TService>
        {
            var headers = this.headerManager.GetServiceHeaders(serviceType.FullName);
            foreach (var header in headers)
            {
                service.HttpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }
}
