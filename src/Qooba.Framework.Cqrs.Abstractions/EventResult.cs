namespace Qooba.Framework.Cqrs.Abstractions
{
    public class EventResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}