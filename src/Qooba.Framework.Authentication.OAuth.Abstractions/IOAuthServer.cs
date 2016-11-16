using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace Qooba.Framework.Authentication.OAuth.Abstractions
{
    public interface IOAuthServer
    {
        void UseOAuthServer(IApplicationBuilder app, IHostingEnvironment env, Func<IConfigurationRoot, IOpenIdConnectServerProvider> oauthProviderImplementationFactory);
    }
}
