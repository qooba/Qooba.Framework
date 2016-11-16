using System;

namespace Qooba.Framework.Services.Abstractions
{
    public interface IRoutingManager
    {
        Uri GetServiceUri(string serviceName);
    }
}
