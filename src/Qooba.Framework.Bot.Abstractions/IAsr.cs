namespace Qooba.Framework.Bot.Abstractions
{
    public interface IAsr
    {
        //TODO:
        AsrResult Process(string text);
    }

    public class AsrResult : NluResult
    {
        public string Text { get; set; }
    }
}
