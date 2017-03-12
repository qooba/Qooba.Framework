using System.Threading.Tasks;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IConnector
    {
        Task<Callback> ReadAsync(IDictionary<string, string[]> headers, string callback);

        Task Send(Reply reply);
    }
}
