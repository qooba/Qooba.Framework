using System;
using AspNet.Security.OpenIdConnect.Server;
using Qooba.Framework.Authentication.OAuth.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Qooba.Framework.Authentication.OAuth
{
    public class OAuthServer : IOAuthServer
    {
        private readonly IOAuthSecurityKeyProvider oauthSecurityKeyProvider;

        public OAuthServer(IOAuthSecurityKeyProvider oauthSecurityKeyProvider)
        {
            this.oauthSecurityKeyProvider = oauthSecurityKeyProvider;
        }

        public void UseOAuthServer(IApplicationBuilder app, IHostingEnvironment env, Func<IConfigurationRoot, IOpenIdConnectServerProvider> oauthProviderImplementationFactory)
        {
            var oauthConfiguration = new ConfigurationBuilder()
                .AddJsonFile("oauth.json")
                .AddJsonFile($"oauth.{env.EnvironmentName}.json", optional: true).Build();

            var key = this.oauthSecurityKeyProvider.Get(oauthConfiguration);
            app.UseOpenIdConnectServer(options =>
            {
                options.ApplicationCanDisplayErrors = true;
                options.AllowInsecureHttp = true;
                options.Provider = oauthProviderImplementationFactory(oauthConfiguration);
                options.TokenEndpointPath = "/token";
                options.AccessTokenLifetime = new TimeSpan(1, 0, 0, 0);
                options.Issuer = new Uri(oauthConfiguration["OAuth:Issuer"]);
                options.AuthenticationScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.SigningCredentials.Clear();
                options.SigningCredentials.Add(new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature));
            });
        }
    }
}
