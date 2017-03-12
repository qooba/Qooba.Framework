using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Qooba.Framework.Bot.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot
{
    public class BotMiddleware
    {
        private readonly RequestDelegate _next;

        public BotMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IBot bot)
        {
            var stream = context.Request.Body;
            var data = new byte[stream.Length];
            stream.Position = 0;
            await stream.ReadAsync(data, 0, (int)stream.Length);
            var path = context.Request.Path.ToString();
            var headers = context.Request?.Headers?.ToDictionary(x => x.Key, x => x.Value.ToArray());
            var callback = Encoding.UTF8.GetString(data);
            Task.Run(() => bot.ProcessAsync(path, headers, callback)).ConfigureAwait(false);
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("OK");
        }
    }

    public static class BotMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCulture(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BotMiddleware>();
        }
    }
}
