using Qooba.Framework.Bot.Connector.Abstractions.Model.Attachments;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Connector.Abstractions.Model
{
    public class EntryMessage : BaseMessage
    {
        public IList<Attachment> Attachments { get; set; }
    }
}
