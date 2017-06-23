using System;

namespace Qooba.Framework.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CompletionActionAttribute : BaseReplyAttribute
    {
        public CompletionActionAttribute(string typeKey) : base(typeKey)
        {
        }

        public CompletionActionAttribute(Type type) : base(type)
        {
        }
    }
}
