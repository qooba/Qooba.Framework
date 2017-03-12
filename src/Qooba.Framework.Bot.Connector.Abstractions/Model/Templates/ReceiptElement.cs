namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Templates
{
    public class ReceiptElement
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public double Quantity { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public string Image_url { get; set; }
    }
}