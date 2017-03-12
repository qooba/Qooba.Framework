namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Buttons
{
    public class LogInButton : Button
    {
        public override ButtonType Type => ButtonType.account_link;
        
        public string Url { get; set; }
    }
}