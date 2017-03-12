namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Templates
{
    public class ReceiptSummary
    {
        public decimal Subtotal { get; set; }

        public decimal Shipping_cost { get; set; }

        public decimal Total_tax { get; set; }

        public decimal Total_cost { get; set; }
    }
}