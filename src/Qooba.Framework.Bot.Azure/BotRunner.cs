using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Azure
{
    public class BotRunner : BaseRunner
    {
        public static async Task Run(string item)
        {
            await ServiceProvider.GetService<IBot>().Run(item);
        }
    }
}
