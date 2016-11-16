using Qooba.Framework.Abstractions;
using Qooba.Framework.Authentication.OAuth;
using Qooba.Framework.Authentication.OAuth.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;

namespace Qooba.Framework.Authentication.OAuth
{
    public class OAuthModule : IModule
    {
        public string Title
        {
            get { return "OAuthModule"; }
        }

        public virtual string Name
        {
            get { return "OAuthModule"; }
        }

        public virtual Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }

        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<IOAuthServer, OAuthServer>(Lifetime.Singleton);
            ContainerManager.Current.RegisterType<IOAuthSecurityKeyProvider, OAuthSecurityKeyProvider>(Lifetime.Singleton);
            ContainerManager.Current.RegisterType<IOAuthBearerToken, OAuthBearerToken>(Lifetime.Singleton);
        }
    }
}
