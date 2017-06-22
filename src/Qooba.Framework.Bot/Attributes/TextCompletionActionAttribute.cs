using System;

namespace Qooba.Framework.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class TextCompletionActionAttribute : CompletionActionAttribute
    {
        private readonly string text;

        public TextCompletionActionAttribute(string text) : base("text")
        {
            this.text = text;
        }

        public override string CompletionActionData => $"{{\"text\":\"{this.text}\"}}";
    }
}