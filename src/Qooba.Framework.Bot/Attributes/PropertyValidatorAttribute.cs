using System;

namespace Qooba.Framework.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PropertyValidatorAttribute : BaseReplyAttribute
    {
        public PropertyValidatorAttribute(string typeKey) : base(typeKey)
        {
        }

        public PropertyValidatorAttribute(Type type) : base(type)
        {
        }
    }
}
