using System;
using Microsoft.AspNet.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Qooba.Framework.Authentication.Abstractions;

namespace Qooba.Framework.Authentication
{
    public class AuthenticationFilter : IAuthenticationFilter
    {
        public void UseAuthenticationFilter(IServiceCollection services, params string[] requiredClaims)
        {
            var policyBuilder = new AuthorizationPolicyBuilder().RequireAuthenticatedUser();
            var policy = requiredClaims.Aggregate(policyBuilder, (p, claim) => p.RequireClaim(claim), x => x.Build());

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new AuthorizeFilter(policy));
            });
        }
    }
}
