//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using Qooba.Framework.Bot.Abstractions;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//namespace Qooba.Framework.Bot
//{
//    public class BotMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public BotMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext context, IConnector connector)
//        {
//            var stream = context.Request.Body;
//            var data = new byte[stream.Length];
//            stream.Position = 0;
//            await stream.ReadAsync(data, 0, (int)stream.Length);
//            Task.Run(() => connector.Process(this.CreateMessage(context.Request))).ConfigureAwait(false);
//            context.Response.StatusCode = 200;
//        }

//        private HttpRequestMessage CreateMessage(HttpRequest request)
//        {
//            var req = new HttpRequestMessage(new HttpMethod(request.Method), request.Path);
//            if (request.Form != null)
//            {
//                req.Content = new FormUrlEncodedContent(request.Form.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)));
//            }
//            else if (request.Body != null)
//            {
//                req.Content = new StreamContent(request.Body);
//            }

//            foreach (var header in request.Headers)
//            {
//                var values = header.Value.ToList();
//                if (!req.Headers.TryAddWithoutValidation(header.Key, values))
//                {
//                    req.Content.Headers.TryAddWithoutValidation(header.Key, values);
//                }
//            }

//            return req;
//        }
//    }

//    public static class BotMiddlewareExtensions
//    {
//        public static IApplicationBuilder UseRequestCulture(
//            this IApplicationBuilder builder)
//        {
//            return builder.UseMiddleware<BotMiddleware>();
//        }
//    }
//}
