using Microsoft.AspNet.Http;
using Qooba.Framework.Web.Abstractions;

namespace Qooba.Framework.Web
{
    public class HeaderReader : IHeaderReader
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public HeaderReader(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string Get(string key)
        {
            if (this.httpContextAccessor != null && this.httpContextAccessor.HttpContext != null && this.httpContextAccessor.HttpContext.Request != null)
            {
                return this.httpContextAccessor.HttpContext.Request.Headers[key];
            }

            return null;
        }
    }
}
