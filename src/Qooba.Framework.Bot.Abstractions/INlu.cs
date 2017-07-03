using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface INlu
    {
        NluResult Process(string text);
    }

    public class NluResult
    {
        public string Intent { get; set; }

        public double Confidence { get; set; }

        public IDictionary<string, object> Entities { get; set; }
    }
}
