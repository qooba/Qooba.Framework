using Qooba.Framework.Bot.Abstractions.Models.Buttons;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions.Models.Templates
{
    public class ButtonTemplateAttachmentPayload : TemplateAttachmentPayload
    {
        public override TemplateAttachmentPayloadType Template_type => TemplateAttachmentPayloadType.button;

        public string Text { get; set; }

        public IList<Button> Buttons { get; set; }
    }
}