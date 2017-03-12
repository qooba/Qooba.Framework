using Qooba.Framework.Bot.Abstractions.Models.Buttons;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions.Models.Templates
{
    public class Element
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Image_url { get; set; }

        public DefaultAction Default_action { get; set; }

        public IList<Button> Buttons { get; set; }
    }
}