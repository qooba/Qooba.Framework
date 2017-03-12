using System;

namespace Qooba.Framework.Abstractions
{
    public interface IContainerBootstrapper : IModule
    {
        IContainer BootstrappContainer();
    }
}