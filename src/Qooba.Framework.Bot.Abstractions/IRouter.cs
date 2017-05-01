using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IRouter
    {
        Task<Route> FindRouteAsync(string text);

        Task<IDictionary<string, object>> FindRouteData(string text, IEnumerable<string> routeTexts);

        int Priority { get; }
    }
}