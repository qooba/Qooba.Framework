namespace Qooba.Framework.Cqrs.Abstractions
{
    public class QueryResult<TResult> : IQueryResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public TResult Data { get; set; }
    }
}