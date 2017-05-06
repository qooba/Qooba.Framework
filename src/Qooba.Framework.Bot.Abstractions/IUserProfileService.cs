using Qooba.Framework.Bot.Abstractions.Models;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IUserProfileService
    {
        Task<User> GetUserAsync(ConnectorType connectorType, string id);

        Task SetUserAsync(User user);
    }
}