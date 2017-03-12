using Qooba.Framework.Abstractions;
using Qooba.Framework.Authentication.Abstractions;

namespace Qooba.Framework.Authentication
{
    public class AuthenticaitonModule : IModule
    {
        public virtual string Name => "AuthenticaitonModule";

        public int Priority => 10;

        public void Bootstrapp(IContainer container)
        {
            container.RegisterType<IAuthenticationFilter, AuthenticationFilter>(Lifetime.Singleton);
        }
    }
}
