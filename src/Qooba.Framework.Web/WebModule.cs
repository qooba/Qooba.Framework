﻿using Qooba.Framework.Abstractions;
using Qooba.Framework.Web.Abstractions;

namespace Qooba.Framework.Web
{
    public class WebModule : IModule
    {
        public virtual string Name => "WebModule";

        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddTransientService<IHeaderReader, HeaderReader>();
        }
    }
}
