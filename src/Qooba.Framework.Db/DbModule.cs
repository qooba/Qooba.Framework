using Qooba.Framework.Abstractions;
using Qooba.Framework.Db.Abstractions;

namespace Qooba.Framework.Db
{
    public class DbModule : IModule
    {
        public virtual string Name => "DbModule";

        public int Priority => 10;

        public void Bootstrapp(IContainer container)
        {
            container.RegisterType<IDbConfig, DbConfig>(Lifetime.Singleton);
        }
    }
}
