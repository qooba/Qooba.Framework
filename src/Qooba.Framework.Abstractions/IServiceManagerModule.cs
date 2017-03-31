using System;

namespace Qooba.Framework.Abstractions
{
    public interface IServiceManagerModule : IModule
    {
        IServiceManager CreateServiceManager();
    }
}