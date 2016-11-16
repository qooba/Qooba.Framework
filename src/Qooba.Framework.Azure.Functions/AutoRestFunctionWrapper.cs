using Qooba.Framework.Azure.Functions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Qooba.Framework.Azure.Functions
{
    public abstract class BaseAutoRestFunction : IAutoRestFunction
    {
        private HttpRequestMessage request;

        public BaseAutoRestFunction(HttpRequestMessage request)
        {
            this.request = request;
        }

        public HttpRequestMessage Request { get { return this.request; } }

        public virtual async Task<HttpResponseMessage> Get(IDictionary<string, string> queryParameters)
        {
            return this.Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
        }

        public virtual async Task<HttpResponseMessage> Post<TRequest>(TRequest request)
        {
            return this.Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
        }

        public virtual async Task<HttpResponseMessage> Put<TRequest>(IDictionary<string, string> queryParameters, TRequest request)
        {
            return this.Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
        }

        public virtual async Task<HttpResponseMessage> Patch<TRequest>(IDictionary<string, string> queryParameters, TRequest request)
        {
            return this.Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
        }

        public virtual async Task<HttpResponseMessage> Delete(IDictionary<string, string> queryParameters)
        {
            return this.Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
        }

        protected async virtual Task<HttpResponseMessage> Delete(HttpRequestMessage req)
        {
            var queryParameters = PrepareQueryParameters(req);
            return await Delete(queryParameters);
        }

        protected async virtual Task<HttpResponseMessage> Get(HttpRequestMessage req, Func<IDictionary<string, string>, Task<HttpResponseMessage>> getMethod)
        {
            var queryParameters = PrepareQueryParameters(req);
            return await Get(queryParameters);
        }

        protected async virtual Task<HttpResponseMessage> Patch<TRequest>(HttpRequestMessage req, Func<IDictionary<string, string>, TRequest, Task<HttpResponseMessage>> patchMethod)
        {
            var request = await PrepareRequest<TRequest>(req);
            var queryParameters = PrepareQueryParameters(req);
            return await Patch(queryParameters, request);
        }

        protected async virtual Task<HttpResponseMessage> Post<TRequest>(HttpRequestMessage req, Func<TRequest, Task<HttpResponseMessage>> postMethod)
        {
            var request = await PrepareRequest<TRequest>(req);
            return await Post(request);
        }

        protected async virtual Task<HttpResponseMessage> Put<TRequest>(HttpRequestMessage req, Func<IDictionary<string, string>, TRequest, Task<HttpResponseMessage>> putMethod)
        {
            var request = await PrepareRequest<TRequest>(req);
            var queryParameters = PrepareQueryParameters(req);
            return await Put(queryParameters, request);
        }

        protected static IDictionary<string, string> PrepareQueryParameters(HttpRequestMessage req)
        {
            return req.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
        }

        protected static async Task<TRequest> PrepareRequest<TRequest>(HttpRequestMessage req)
        {
            string jsonContent = await req.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TRequest>(jsonContent);
        }
    }
}
