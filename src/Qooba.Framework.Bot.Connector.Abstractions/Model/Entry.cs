using System.Collections.Generic;

namespace Qooba.Framework.Bot.Connector.Abstractions.Model
{
    public class Entry
    {
        public string Id { get; set; }
        
        public int Time { get; set; }

        public IList<Messaging> Messaging { get; set; }
    }
}
