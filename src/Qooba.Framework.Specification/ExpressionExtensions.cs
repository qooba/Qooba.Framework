using System;
using System.Linq.Expressions;

namespace Qooba.Framework.Specification
{
    public static class ExpressionExtensions
    {
        public static string ToPropertyName<T>(this Expression<Func<T, object>> selector)
        {
            var memberExpression = selector.Body as MemberExpression;
            if(memberExpression == null)
            {
                throw new ArgumentNullException(string.Concat(typeof(MemberExpression).FullName, " exprected"));
            }

            return memberExpression.Member.Name;
            //return memberExpression.ToString().Remove(0, 2);
        }
    }
}
