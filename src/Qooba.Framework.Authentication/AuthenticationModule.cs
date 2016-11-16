using Qooba.Framework.Abstractions;
using Qooba.Framework.Authentication.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;

namespace Qooba.Framework.Authentication
{
    public class AuthenticaitonModule : IModule
    {
        public virtual string Name
        {
            get { return "AuthenticaitonModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<IAuthenticationFilter, AuthenticationFilter>(Lifetime.Singleton);
        }
    }
}
