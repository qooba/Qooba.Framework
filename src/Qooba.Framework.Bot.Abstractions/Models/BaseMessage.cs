using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions.Models
{
    public abstract class BaseMessage
    {
        public string Text { get; set; }
        
        public IList<QuickReply> Quick_replies { get; set; }
    }
}
