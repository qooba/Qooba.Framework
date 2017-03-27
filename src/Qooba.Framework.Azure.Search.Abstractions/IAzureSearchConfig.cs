namespace Qooba.Framework.Azure.Search.Abstractions
{
    public interface IAzureSearchConfig
    {
        string SearchApiKey { get; }

        string SearchServiceName { get; }
    }
}
