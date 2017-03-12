using System.Collections.Generic;

namespace Qooba.Framework.Bot.Connector.Abstractions.Model
{
    public abstract class BaseMessage
    {
        public string Text { get; set; }
        
        public IList<QuickReply> Quick_replies { get; set; }
    }
}
