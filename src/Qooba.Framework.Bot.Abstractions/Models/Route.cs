using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions.Models
{
    public class Route
    {
        public string RouteId { get; set; }

        public string RouteText { get; set; }

        public IDictionary<string, object> RouteData { get; set; }

        public bool IsDefault { get; set; }
    }
}