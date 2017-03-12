namespace Qooba.Framework.Bot.Abstractions.Models.Buttons
{
    public class LogInButton : Button
    {
        public override ButtonType Type => ButtonType.account_link;
        
        public string Url { get; set; }
    }
}