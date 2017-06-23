using System;

namespace Qooba.Framework.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HttpCompletionActionAttribute : CompletionActionAttribute
    {
        private readonly string url;

        public HttpCompletionActionAttribute(string url) : base("http")
        {
            this.url = url;
        }

        public override string Data => $"{{\"url\":\"{this.url}\"}}";
    }
}
