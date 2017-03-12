using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IBot
    {
        Task ProcessAsync(string path, IDictionary<string,string[]> headers, string callback);
    }
}
