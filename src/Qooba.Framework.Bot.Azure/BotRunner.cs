using Microsoft.Azure.WebJobs.Host;
using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Azure
{
    public class BotRunner : BaseRunner
    {
        public static async Task Run(string item, TraceWriter log)
        {
            await Container.Resolve<IBot>().Run(item);
        }
    }
}
