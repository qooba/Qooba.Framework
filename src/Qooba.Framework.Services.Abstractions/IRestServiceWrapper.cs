using System.Threading.Tasks;
using Microsoft.Rest;
using System;
using Qooba.Framework.Services.Abstractions.Models;

namespace Qooba.Framework.Services.Abstractions
{
    public interface IRestServiceFactory
    {
        void Register<TServiceInterface, TService>(Func<Uri, TService> serviceFactory)
            where TService : ServiceClient<TService>, TServiceInterface
            where TServiceInterface : IDisposable;

        Task<TResponse> Invoke<TServiceInterface, TResponse>(Func<TServiceInterface, Task<TResponse>> action)
            where TServiceInterface : IDisposable;

        Task<TOutput> Invoke<TServiceInterface, TInput, TRequest, TResponse, TOutput>(Func<TServiceInterface, TRequest, Task<TResponse>> action, TInput input)
            where TServiceInterface : IDisposable
            where TOutput : BaseOutput;

        Task<TOutput> Invoke<TServiceInterface, TResponse, TOutput>(Func<TServiceInterface, Task<TResponse>> action)
            where TServiceInterface : IDisposable
            where TOutput : BaseOutput;
    }
}
