using System;

namespace Qooba.Framework.Abstractions
{
    public interface IModuleDescriptor
    {
        IModuleDescriptor Module<TModule>() where TModule : class, IModule;

        IModuleDescriptor Module(Type moduleType);

        IModuleDescriptor Module(object moduleInstance);

        IModuleDescriptor Module<TModule>(TModule moduleInstance) where TModule : class, IModule;
    }
}
