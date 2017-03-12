using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions.Models.Templates
{
    public class GenericTemplateAttachmentPayload : TemplateAttachmentPayload
    {
        public override TemplateAttachmentPayloadType Template_type => TemplateAttachmentPayloadType.generic;

        public ImageAspectRatio Image_aspect_ratio { get; set; }

        public IList<Element> Elements { get; set; }
    }
}