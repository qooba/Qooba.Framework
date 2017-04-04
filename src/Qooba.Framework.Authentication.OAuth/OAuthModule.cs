using Qooba.Framework.Abstractions;
using Qooba.Framework.Authentication.OAuth.Abstractions;

namespace Qooba.Framework.Authentication.OAuth
{
    public class OAuthModule : IModule
    {
        public virtual string Name => "OAuthModule";

        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddSingletonService<IOAuthServer, OAuthServer>();
            framework.AddSingletonService<IOAuthSecurityKeyProvider, OAuthSecurityKeyProvider>();
            framework.AddSingletonService<IOAuthBearerToken, OAuthBearerToken>();
        }
    }
}
