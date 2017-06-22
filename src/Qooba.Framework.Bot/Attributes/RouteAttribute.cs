using System;

namespace Qooba.Framework.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RouteAttribute : Attribute
    {
        public RouteAttribute(string route)
        {
            this.Route = route;
        }

        public string Route { get; private set; }
    }
}
