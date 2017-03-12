using Qooba.Framework.Bot.Abstractions.Models.Attachments;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions.Models
{
    public class EntryMessage : BaseMessage
    {
        public IList<Attachment> Attachments { get; set; }
    }
}
