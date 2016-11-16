using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Qooba.Framework.Dnx.Abstractions
{
    public interface IAppConfiguration
    {
        int Priority { get; }

        void ConfigureServices(IServiceCollection services);

        void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory);
    }
}
