using System.Collections.Generic;

namespace Qooba.Framework.Bot.Connector.Abstractions.Model
{
    public class Callback
    {
        public string Object { get; set; }

        public IList<Entry> Entry { get; set; }
    }
}
