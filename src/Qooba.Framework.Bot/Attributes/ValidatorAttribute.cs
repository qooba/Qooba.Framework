using System;

namespace Qooba.Framework.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public abstract class ValidatorAttribute : Attribute
    {
        public ValidatorAttribute(string validatorType)
        {
            this.ValidatorType = validatorType;
        }

        public string ValidatorType { get; private set; }

        public abstract string ValidatorData { get; }
    }
}
