namespace Qooba.Framework.Bot.Connector.Abstractions.Model
{
    public class QuickReply
    {
        public ContentType Content_type { get; set; }

        public string Title { get; set; }

        public string Payload { get; set; }

        public string Image_url { get; set; }
    }
}