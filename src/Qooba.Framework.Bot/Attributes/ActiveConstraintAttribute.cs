using System;

namespace Qooba.Framework.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ActiveConstraintAttribute : Attribute
    {
        public ActiveConstraintAttribute(string activeConstraintType)
        {
            this.ActiveConstraintType = activeConstraintType;
        }

        public string ActiveConstraintType { get; private set; }

        public virtual string ActiveConstraintData { get; }
    }
}
