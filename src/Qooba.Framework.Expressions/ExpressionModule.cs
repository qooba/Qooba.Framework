using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.Expressions;
using Qooba.Framework.Expressions.Abstractions;
using System;

namespace Qooba.Framework.Expression
{
    public class ExpressionModule : IModule
    {
        public virtual string Name
        {
            get { return "ExpressionModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<IExpressionHelper, ExpressionHelper>();
        }
    }
}
