using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Reflection;
using Qooba.Framework.Abstractions;
using System;
#if NET46
#else
using Microsoft.Extensions.DependencyModel;
#endif
using System.IO;

namespace Qooba.Framework
{
    internal class ModuleManager : IModuleManager, IModuleBootstrapper
    {
        internal static IDictionary<IModule, Assembly> Modules = new ConcurrentDictionary<IModule, Assembly>();
        
        public IModuleManager AddModule(IModule module)
        {
            Modules.Add(module, null);
            return this;
        }

        public IList<IModule> GetModules() => Modules.OrderBy(x => x.Key.Priority).Select(x => x.Key).ToList();

        public IModule GetModule(string name) => Modules.Where(x => x.Key.Name == name).Select(x => x.Key).FirstOrDefault();
        
        public IModuleManager AddModule(Func<IModuleDescriptor, IModuleDescriptor> moduleDescriptorFactory)
        {
            ModuleDescriptor descriptor = moduleDescriptorFactory(new ModuleDescriptor()) as ModuleDescriptor;
            this.AddModule(descriptor.Instance);
            return this;
        }
    }
}
