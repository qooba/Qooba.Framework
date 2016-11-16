using System.Threading.Tasks;

namespace Qooba.Framework.Azure.IoT.Abstractions
{
    public interface IIoTHub
    {
        Task Send<T>(string deviceId, T message);
    }
}
