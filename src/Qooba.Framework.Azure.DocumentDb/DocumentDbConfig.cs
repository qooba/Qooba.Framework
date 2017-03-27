using Qooba.Framework.Azure.DocumentDb.Abstractions;
using Qooba.Framework.Configuration.Abstractions;

namespace Qooba.Framework.Azure.DocumentDb
{
    public class DocumentDbConfig : IDocumentDbConfig
    {
        private readonly IConfig config;

        public DocumentDbConfig(IConfig config)
        {
            this.config = config;
        }

        public string DocumentDbUri => this.config["Data:DefaultConnection:DocumentDbUri"];

        public string DocumentDbPrimaryKey => this.config["Data:DefaultConnection:DocumentDbPrimaryKey"];

        public string DocumentDbDatabaseName => this.config["Data:DefaultConnection:DocumentDbDatabaseName"];

        public string DocumentDbCollectionName => this.config["Data:DefaultConnection:DocumentDbCollectionName"];
    }
}