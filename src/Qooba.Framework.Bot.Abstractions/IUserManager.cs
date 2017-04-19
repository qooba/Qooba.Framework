using Qooba.Framework.Bot.Abstractions.Models;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IUserManager
    {
        Task<User> GetUserAsync(string id);
    }
}