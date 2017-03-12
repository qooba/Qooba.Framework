using System.Threading.Tasks;
using Qooba.Framework.Bot.Connector.Abstractions.Model;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Connector.Abstractions
{
    public interface IConnector
    {
        Task<Callback> ReadAsync(IDictionary<string, string[]> headers, string callback);

        Task Send(Reply reply);
    }
}
