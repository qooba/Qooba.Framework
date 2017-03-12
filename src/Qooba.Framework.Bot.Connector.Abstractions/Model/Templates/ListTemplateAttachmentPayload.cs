using Qooba.Framework.Bot.Connector.Abstractions.Model.Buttons;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Templates
{
    public class ListTemplateAttachmentPayload : TemplateAttachmentPayload
    {
        public override TemplateAttachmentPayloadType Template_type => TemplateAttachmentPayloadType.list;

        public TopElementStyle Top_element_style { get; set; }

        public IList<Element> Elements { get; set; }

        public IList<Button> Buttons { get; set; }
    }
}