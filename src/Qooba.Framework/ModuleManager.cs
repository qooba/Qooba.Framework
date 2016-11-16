using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Reflection;
using Qooba.Framework.Abstractions;
using System;

namespace Qooba.Framework
{
    public class ModuleManager
    {
        internal IDictionary<IModule, Assembly> Modules { get; set; }

        public ModuleManager()
        {
            Modules = new ConcurrentDictionary<IModule, Assembly>();
        }

        private static Lazy<ModuleManager> current = new Lazy<ModuleManager>(() => new ModuleManager());

        public static ModuleManager Current => current.Value;

        public IList<IModule> GetModules() => Modules.OrderBy(x => x.Key.Priority).Select(x => x.Key).ToList();

        public IModule GetModule(string name) => Modules.Where(x => x.Key.Name == name).Select(x => x.Key).FirstOrDefault();

        public void Bootstrapp()
        {
            var modules = GetModules();
            foreach (var module in modules)
            {
                module.Bootstrapp();
            }
        }
    }
}
