using System.Collections.Generic;

namespace Qooba.Framework.Services.Abstractions
{
    public interface IHeaderManager
    {
        IDictionary<string,string> GetServiceHeaders(string serviceName);
    }
}
