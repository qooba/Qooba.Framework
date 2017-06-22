using System;

namespace Qooba.Framework.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CompletionActionAttribute : Attribute
    {
        public CompletionActionAttribute(string completionActionType)
        {
            this.CompletionActionType = completionActionType;
        }

        public string CompletionActionType { get; private set; }

        public virtual string CompletionActionData { get; }
    }
}
