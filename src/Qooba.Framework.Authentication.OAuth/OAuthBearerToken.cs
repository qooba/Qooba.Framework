using System;
using Qooba.Framework.Authentication.OAuth.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Qooba.Framework.Authentication.OAuth
{
    public class OAuthBearerToken : IOAuthBearerToken
    {
        private readonly IOAuthSecurityKeyProvider oauthSecurityKeyProvider;

        public OAuthBearerToken(IOAuthSecurityKeyProvider oauthSecurityKeyProvider)
        {
            this.oauthSecurityKeyProvider = oauthSecurityKeyProvider;
        }

        public void UseJwtBearerAuthentication(IApplicationBuilder app, IHostingEnvironment env)
        {
            var oauthConfiguration = new ConfigurationBuilder()
                .AddJsonFile("oauth.json")
                .AddJsonFile($"oauth.{env.EnvironmentName}.json", optional: true).Build();

            var key = this.oauthSecurityKeyProvider.Get(oauthConfiguration);
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                Audience = oauthConfiguration["OAuth:TokenAudience"],
                Authority = oauthConfiguration["OAuth:Issuer"],
                AutomaticAuthenticate = true,
                RequireHttpsMetadata = false,
                TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidAudience = oauthConfiguration["OAuth:TokenIssuer"],
                    ValidIssuer = oauthConfiguration["OAuth:TokenIssuer"],
                    //TODO: RC2
                    //ValidateSignature = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }
            });
        }
    }
}
