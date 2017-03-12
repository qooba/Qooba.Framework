using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions.Models.Templates
{
    public class AirlineItenaryTemplateAttachmentPayload : TemplateAttachmentPayload
    {
        public override TemplateAttachmentPayloadType Template_type => TemplateAttachmentPayloadType.airline_itinerary;

        public string Intro_message { get; set; }

        public string Locale { get; set; }

        public string Theme_color { get; set; }

        public string Pnr_number { get; set; }

        public IList<PassangerInfo> Passanger_info { get; set; }

        public IList<FlightInfo> Flight_info { get; set; }

        public IList<PassangerSegmentInfo> Passanger_segment_info { get; set; }

        public IList<PriceInfo> Price_info { get; set; }

        public decimal Base_price { get; set; }

        public decimal Tax { get; set; }

        public decimal Total_price { get; set; }

        public string Currency { get; set; }
    }
}