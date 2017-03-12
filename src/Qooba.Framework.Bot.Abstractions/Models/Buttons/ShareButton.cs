namespace Qooba.Framework.Bot.Abstractions.Model.Buttons
{
    public class ShareButton : Button
    {
        public override ButtonType Type => ButtonType.element_share;
        
        public string Share_contents { get; set; }
    }
}