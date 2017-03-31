using System;
using Qooba.Framework.Abstractions;

namespace Qooba.Framework
{
    internal class ModuleDescriptor : IModuleDescriptor
    {
        public IModuleDescriptor Module(Type moduleType)
        {
            this.Instance = (IModule)Activator.CreateInstance(moduleType);
            return this;
        }

        public IModuleDescriptor Module(object moduleInstance)
        {
            var module = moduleInstance as IModule;
            this.Instance = module ?? throw new InvalidOperationException("Upps ... module must implement IModule interface");
            return this;
        }

        IModuleDescriptor IModuleDescriptor.Module<TModule>() => this.Module(typeof(TModule));
        
        IModuleDescriptor IModuleDescriptor.Module<TModule>(TModule moduleInstance)
        {
            this.Instance = moduleInstance;
            return this;
        }

        internal IModule Instance { get; private set; }
    }
}
