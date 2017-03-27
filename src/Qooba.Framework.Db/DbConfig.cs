using Qooba.Framework.Db.Abstractions;
using Qooba.Framework.Configuration.Abstractions;

namespace Qooba.Framework.Db
{
    public class DbConfig : IDbConfig
    {
        private IConfig config;

        public DbConfig(IConfig config)
        {
            this.config = config;
        }

        public string ConnectionString => this.config["Data:DefaultConnection:ConnectionString"];
    }
}