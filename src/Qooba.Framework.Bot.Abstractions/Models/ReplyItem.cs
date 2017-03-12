using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions.Models
{
    public class ReplyItem
    {
        public string ReplyId { get; set; }

        public IList<string> Routes { get; set; }

        public string ReplyType { get; set; }

        public JObject Reply { get; set; }
    }
}