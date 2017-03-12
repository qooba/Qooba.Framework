using Qooba.Framework.Abstractions;
using Qooba.Framework.Expressions;
using Qooba.Framework.Expressions.Abstractions;

namespace Qooba.Framework.Expression
{
    public class ExpressionModule : IModule
    {
        public virtual string Name => "ExpressionModule";

        public int Priority => 10;

        public void Bootstrapp(IContainer container)
        {
            container.RegisterType<IExpressionHelper, ExpressionHelper>();
        }
    }
}
