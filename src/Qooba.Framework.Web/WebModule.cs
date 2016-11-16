using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.Web.Abstractions;
using System;

namespace Qooba.Framework.Web
{
    public class WebModule : IModule
    {
        public virtual string Name
        {
            get { return "WebModule"; }
        }
        
        public int Priority
        {
            get { return 10; }
        }

        public void Bootstrapp()
        {
            ContainerManager.Current.RegisterType<IHeaderReader, HeaderReader>();
        }
    }
}
