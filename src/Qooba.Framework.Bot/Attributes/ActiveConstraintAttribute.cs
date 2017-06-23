using System;

namespace Qooba.Framework.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ActiveConstraintAttribute : BaseReplyAttribute
    {
        public ActiveConstraintAttribute(string typeKey) : base(typeKey)
        {
        }

        public ActiveConstraintAttribute(Type type) : base(type)
        {
        }
    }
}
