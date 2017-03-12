namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Buttons
{
    public class PostbackButton : Button
    {
        public override ButtonType Type => ButtonType.postback;
        
        public string Title { get; set; }
        
        public string Payload { get; set; }
    }
}