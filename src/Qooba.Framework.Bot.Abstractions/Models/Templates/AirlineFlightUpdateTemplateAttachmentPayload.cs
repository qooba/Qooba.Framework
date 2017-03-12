using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions.Models.Templates
{
    public class AirlineFlightUpdateTemplateAttachmentPayload : TemplateAttachmentPayload
    {
        public override TemplateAttachmentPayloadType Template_type => TemplateAttachmentPayloadType.airline_update;

        public string Intro_message { get; set; }

        public UpdateType Update_type { get; set; }

        public string Locale { get; set; }

        public string Theme_color { get; set; }

        public string Pnr_number { get; set; }
        
        public IList<UpdateFlightInfo> Update_flight_info { get; set; }
    }
}