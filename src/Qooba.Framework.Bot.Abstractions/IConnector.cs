using System.Threading.Tasks;
using System.Net.Http;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IConnector
    {
        Task<HttpResponseMessage> Process(HttpRequestMessage req);
    }
}
