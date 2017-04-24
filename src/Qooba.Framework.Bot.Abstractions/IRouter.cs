using Qooba.Framework.Bot.Abstractions.Models;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IRouter
    {
        Task<Route> FindRouteAsync(string text);

        int Priority { get; }
    }
}