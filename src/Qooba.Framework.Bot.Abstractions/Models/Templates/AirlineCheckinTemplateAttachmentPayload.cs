using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions.Models.Templates
{
    public class AirlineCheckinTemplateAttachmentPayload : TemplateAttachmentPayload
    {
        public override TemplateAttachmentPayloadType Template_type => TemplateAttachmentPayloadType.airline_checkin;

        public string Intro_message { get; set; }

        public string Locale { get; set; }

        public string Theme_color { get; set; }

        public string Pnr_number { get; set; }

        public IList<FlightInfo> Flight_info { get; set; }

        public string Checkin_url { get; set; }
    }
}