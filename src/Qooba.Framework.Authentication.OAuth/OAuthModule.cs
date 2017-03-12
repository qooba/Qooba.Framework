using Qooba.Framework.Abstractions;
using Qooba.Framework.Authentication.OAuth.Abstractions;

namespace Qooba.Framework.Authentication.OAuth
{
    public class OAuthModule : IModule
    {
        public virtual string Name => "OAuthModule";

        public int Priority => 10;
        
        public void Bootstrapp(IContainer container)
        {
            container.RegisterType<IOAuthServer, OAuthServer>(Lifetime.Singleton);
            container.RegisterType<IOAuthSecurityKeyProvider, OAuthSecurityKeyProvider>(Lifetime.Singleton);
            container.RegisterType<IOAuthBearerToken, OAuthBearerToken>(Lifetime.Singleton);
        }
    }
}
