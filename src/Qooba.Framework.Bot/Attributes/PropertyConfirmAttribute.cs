using System;

namespace Qooba.Framework.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PropertyConfirmAttribute : BaseReplyAttribute
    {
        public PropertyConfirmAttribute(string typeKey) : base(typeKey)
        {
        }

        public PropertyConfirmAttribute(Type type) : base(type)
        {
        }
    }
}
