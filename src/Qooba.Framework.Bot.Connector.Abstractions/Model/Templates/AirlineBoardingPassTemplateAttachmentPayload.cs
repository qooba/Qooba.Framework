using Qooba.Framework.Bot.Connector.Abstractions.Model.Buttons;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Templates
{
    public class AirlineBoardingPassTemplateAttachmentPayload : TemplateAttachmentPayload
    {
        public override TemplateAttachmentPayloadType Template_type => TemplateAttachmentPayloadType.airline_boardingpass;

        public string Intro_message { get; set; }

        public string Locale { get; set; }

        public string Theme_color { get; set; }

        public IList<BoardingPass> Boarding_pass { get; set; }
    }
}