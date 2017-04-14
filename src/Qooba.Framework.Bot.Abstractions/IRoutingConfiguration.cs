using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IRoutingConfiguration
    {
        IList<Route> RoutingTable { get; }
    }
}