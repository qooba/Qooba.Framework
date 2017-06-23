using System;

namespace Qooba.Framework.Bot.Attributes
{
    public abstract class BaseReplyAttribute : Attribute
    {
        public BaseReplyAttribute(string typeKey)
        {
            if (string.IsNullOrEmpty(typeKey))
            {
                throw new ArgumentNullException(nameof(typeKey));
            }

            this.TypeKey = typeKey;
        }

        public BaseReplyAttribute(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.Type = type;
            this.TypeKey = type.FullName;
        }

        public string TypeKey { get; private set; }

        public Type Type { get; private set; }

        public virtual string Data { get; }
    }
}
