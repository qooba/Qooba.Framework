using Qooba.Framework.Bot.Connector.Abstractions.Model.Buttons;

namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Templates
{
    public class DefaultAction : Button
    {
        public override ButtonType Type => ButtonType.web_url;

        public string Url { get; set; }
        
        public WebviewHeightRatio Webview_height_ratio { get; set; }

        public bool Messenger_extensions { get; set; }

        public string Fallback_url { get; set; }

        public string Webview_share_button { get; set; }
    }
}