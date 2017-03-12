using Qooba.Framework.Bot.Connector.Abstractions.Model.Buttons;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Templates
{
    public class ReceiptTemplateAttachmentPayload : TemplateAttachmentPayload
    {
        public override TemplateAttachmentPayloadType Template_type => TemplateAttachmentPayloadType.receipt;

        public string Recipient_name { get; set; }

        public string Merchant_name { get; set; }

        public string Order_number { get; set; }

        public string Currency { get; set; }

        public string Payment_method { get; set; }

        public string Timestamp { get; set; }

        public string Order_url { get; set; }
        
        public IList<ReceiptElement> Elements { get; set; }

        public ReceiptAddress Address { get; set; }

        public ReceiptSummary Summary { get; set; }

        public ReceiptAdjustments Adjustments { get; set; }
    }
}