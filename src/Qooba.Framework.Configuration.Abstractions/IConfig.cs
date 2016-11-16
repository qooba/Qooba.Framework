namespace Qooba.Framework.Configuration.Abstractions
{
    public interface IConfig
    {
        string ConnectionString { get; }

        string StorageConnectionString { get; }

        string SearchApiKey { get; }

        string SearchServiceName { get; }

        string DocumentDbUri { get; }

        string DocumentDbPrimaryKey { get; }

        string DocumentDbDatabaseName { get; }

        string DocumentDbCollectionName { get; }

        string this[string key] { get; }
    }
}
