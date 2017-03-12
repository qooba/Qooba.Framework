namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Buttons
{
    public class UrlButton : Button
    {
        public override ButtonType Type => ButtonType.web_url;

        public string Url { get; set; }

        public string Title { get; set; }

        public WebviewHeightRatio Webview_height_ratio { get; set; }
        
        public bool Messenger_extensions { get; set; }

        public string Fallback_url { get; set; }

        public string Webview_share_button { get; set; }
    }
}