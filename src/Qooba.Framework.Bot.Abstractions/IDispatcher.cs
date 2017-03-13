using Qooba.Framework.Bot.Abstractions.Models;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IDispatcher
    {
        ConnectorType ConnectorType { get; }

        Task SendAsync(Reply reply);
    }
}