using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Qooba.Framework.Authentication.OAuth.Abstractions
{
    public interface IOAuthBearerToken
    {
        void UseJwtBearerAuthentication(IApplicationBuilder app, IHostingEnvironment env);
    }
}
