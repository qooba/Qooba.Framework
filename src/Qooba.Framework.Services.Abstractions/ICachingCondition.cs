using System;

namespace Qooba.Framework.Services.Abstractions
{
    public interface ICachingCondition
    {
        Func<object, bool> IsSatisfiedBy();
    }
}
