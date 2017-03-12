namespace Qooba.Framework.Bot.Abstractions.Models.Buttons
{
    public class CallButton : Button
    {
        public override ButtonType Type => ButtonType.phone_number;
        
        public string Title { get; set; }
        
        public string Payload { get; set; }
    }
}