namespace Qooba.Framework.Azure.DocumentDb.Abstractions
{
    public interface IDocumentDbConfig
    {
        string DocumentDbUri { get; }

        string DocumentDbPrimaryKey { get; }

        string DocumentDbDatabaseName { get; }

        string DocumentDbCollectionName { get; }
    }
}
