using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Qooba.Framework.Azure.Functions.Abstractions
{
    public interface IAutoRestFunction
    {
        Task<HttpResponseMessage> Get(IDictionary<string, string> queryParameters);

        Task<HttpResponseMessage> Post<TRequest>(TRequest request);

        Task<HttpResponseMessage> Put<TRequest>(IDictionary<string, string> queryParameters, TRequest request);

        Task<HttpResponseMessage> Patch<TRequest>(IDictionary<string, string> queryParameters, TRequest request);

        Task<HttpResponseMessage> Delete(IDictionary<string, string> queryParameters);
    }
}
