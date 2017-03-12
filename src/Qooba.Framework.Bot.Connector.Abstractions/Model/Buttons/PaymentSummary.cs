using System.Collections.Generic;

namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Buttons
{
    public class PaymentSummary
    {
        public string Currency { get; set; }

        public bool Is_test_payment { get; set; }

        public PaymentType Payment_type { get; set; }

        public string Merchant_name { get; set; }

        public IList<RequestedUserInfo> Requested_user_info { get; set; }

        public IList<PriceInfo> Price_list { get; set; }
    }
}