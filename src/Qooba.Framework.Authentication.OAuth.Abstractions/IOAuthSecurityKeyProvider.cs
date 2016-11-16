using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Qooba.Framework.Authentication.OAuth.Abstractions
{
    public interface IOAuthSecurityKeyProvider
    {
        SecurityKey Get(IConfigurationRoot oauthConfiguration);
    }
}
