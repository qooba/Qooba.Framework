using System;

namespace Qooba.Framework.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyReplyAttribute : BaseReplyAttribute
    {
        public PropertyReplyAttribute(string typeKey) : base(typeKey)
        {
        }

        public PropertyReplyAttribute(Type type) : base(type)
        {
        }
    }
}
