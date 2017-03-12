namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Buttons
{
    public class BuyButton : Button
    {
        public override ButtonType Type => ButtonType.payment;
        
        public string Title { get; set; }

        public string Payload { get; set; }

        public PaymentSummary Payment_summary { get; set; }
    }
}