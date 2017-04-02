﻿using Qooba.Framework.Abstractions;
using Qooba.Framework.Logging.Abstractions;

namespace Qooba.Framework.Logging.AzureApplicationInsights
{
    public class AzureApplicationInsightsModule : IModule
    {
        public virtual string Name => "AzureApplicationInsightsModule";

        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddTransientService<ILogger, AzureApplicationInsightsLogger>();
        }
    }
}
