using System;

namespace Qooba.Framework.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyReplyAttribute : Attribute
    {
        public PropertyReplyAttribute(string replyType)
        {
            this.ReplyType = replyType;
        }

        public string ReplyType { get; private set; }

        public virtual string Reply { get; }
    }
}
