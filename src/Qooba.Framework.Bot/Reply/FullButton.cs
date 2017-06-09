using Qooba.Framework.Bot.Abstractions.Models.Buttons;

namespace Qooba.Framework.Bot
{
    public class FullButton
    {
        public ButtonType Type { get; set; }

        public string Title { get; set; }

        public string Payload { get; set; }

        public string Url { get; set; }

        public string Share_contents { get; set; }

        public WebviewHeightRatio Webview_height_ratio { get; set; }

        public bool Messenger_extensions { get; set; }

        public string Fallback_url { get; set; }

        public string Webview_share_button { get; set; }
        
        public PaymentSummary Payment_summary { get; set; }

        public Button ToButton()
        {
            if(this.Type == ButtonType.web_url)
            {
                return new UrlButton
                {
                    Fallback_url = this.Fallback_url,
                    Messenger_extensions = this.Messenger_extensions,
                    Title = this.Title,
                    Url = this.Url,
                    Webview_height_ratio = this.Webview_height_ratio,
                    Webview_share_button = this.Webview_share_button
                };
            }

            if (this.Type == ButtonType.postback)
            {
                return new PostbackButton
                {
                    Payload = this.Payload,
                    Title = this.Title
                };
            }

            if (this.Type == ButtonType.element_share)
            {
                return new ShareButton
                {
                    Share_contents = this.Share_contents
                };
            }

            if (this.Type == ButtonType.phone_number)
            {
                return new CallButton
                {
                    Payload = this.Payload,
                    Title = this.Title
                };
            }

            if (this.Type == ButtonType.account_link)
            {
                return new LogInButton
                {
                    Url = this.Url
                };
            }

            if (this.Type == ButtonType.account_unlink)
            {
                return new LogOutButton();
            }

            if (this.Type == ButtonType.payment)
            {
                return new BuyButton
                {
                    Payload = this.Payload,
                    Payment_summary = this.Payment_summary,
                    Title = this.Title
                };
            }

            return null;
        }
    }
}
