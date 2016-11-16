using System.Collections.Generic;
using System.Linq.Expressions;

namespace Qooba.Framework.Specification
{
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression expression) => new ParameterRebinder(map).Visit(expression);
        
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replecement;
            if (_map.TryGetValue(p, out replecement))
            {
                p = replecement;
            }
            return base.VisitParameter(p);
        }
    }
}
