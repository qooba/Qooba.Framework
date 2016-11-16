using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Qooba.Framework.Authentication.OAuth.Abstractions;
using System.Security.Cryptography;

namespace Qooba.Framework.Authentication.OAuth
{
    public class OAuthSecurityKeyProvider : IOAuthSecurityKeyProvider
    {
        public SecurityKey Get(IConfigurationRoot oauthConfiguration)
        {
            var publicAndPrivate = new RSACryptoServiceProvider();
            //TODO: RC2
            //publicAndPrivate.FromXmlString(oauthConfiguration["OAuth:Key"]);
            return new RsaSecurityKey(publicAndPrivate.ExportParameters(true));
        }
    }
}
