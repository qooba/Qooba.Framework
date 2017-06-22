using System;

namespace Qooba.Framework.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class HttpCompletionActionAttribute : CompletionActionAttribute
    {
        private readonly string url;

        public HttpCompletionActionAttribute(string url) : base("http")
        {
            this.url = url;
        }

        public override string CompletionActionData => $"{{\"url\":\"{this.url}\"}}";
    }
}
