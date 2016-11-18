using System;
using System.Linq.Expressions;

namespace Qooba.Framework.Cqrs.Abstractions
{
    public class QueryFilterParameter<TParameter, TResult> : QueryFilterParameter<TParameter>
        where TParameter : class
    {
        public Expression<Func<TParameter, TResult>> Selector { get; set; }
    }
}