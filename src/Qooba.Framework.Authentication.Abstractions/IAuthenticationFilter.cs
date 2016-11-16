using Microsoft.Extensions.DependencyInjection;

namespace Qooba.Framework.Authentication.Abstractions
{
    public interface IAuthenticationFilter
    {
        void UseAuthenticationFilter(IServiceCollection services, params string[] requiredClaims);
    }
}
