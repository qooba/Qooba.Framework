using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.Dnx.Abstractions;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Qooba.IoT.Frontend.Bootstrap
{
    public class Statup
    {
        public Statup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public virtual IConfigurationRoot Configuration { get; set; }

        public List<IAppConfiguration> AppConfigurations { get; private set; }

        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.Configuration);
            Framework.Bootstrapper.Bootstrapp();
            ContainerManager.Current.Populate(services);
            this.AppConfigurations = ContainerManager.Current.ResolveAll<IAppConfiguration>().OrderBy(x => x.Priority).ToList();
            this.AppConfigurations.ForEach(x => x.ConfigureServices(services));
            return ContainerManager.Current.Resolve<IServiceProvider>();
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            this.AppConfigurations.ForEach(x => x.Configure(app, env, loggerFactory));
        }
    }
}
