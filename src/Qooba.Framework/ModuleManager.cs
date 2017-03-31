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
        public const string MODULE_NAME_PATTERN = "Qooba";

        internal IDictionary<IModule, Assembly> Modules { get; set; }

        public ModuleManager()
        {
            Modules = new ConcurrentDictionary<IModule, Assembly>();
        }

        private static Lazy<ModuleManager> current = new Lazy<ModuleManager>(() => new ModuleManager());

        public static ModuleManager Current => current.Value;

        public IModuleManager AddModule(IModule module)
        {
            this.Modules.Add(module, null);
            return this;
        }

        public IList<IModule> GetModules() => Modules.OrderBy(x => x.Key.Priority).Select(x => x.Key).ToList();

        public IModule GetModule(string name) => Modules.Where(x => x.Key.Name == name).Select(x => x.Key).FirstOrDefault();

        public void BootstrappModules(params string[] includeModuleNamePattern)
        {
#if NET46
            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var d = new System.IO.DirectoryInfo(path);
            var assemblies = d.GetFiles("*.dll", System.IO.SearchOption.AllDirectories)
                .Where(x => x.FullName.Contains(MODULE_NAME_PATTERN) || includeModuleNamePattern.Any(i => x.FullName.Contains(i)))
                .Select(x => AssemblyName.GetAssemblyName(x.FullName)).Where(x => x.FullName.StartsWith(MODULE_NAME_PATTERN) || includeModuleNamePattern.Any(i => x.FullName.Contains(i)))
                .Select(x => Assembly.Load(x.FullName));

            foreach (var a in assemblies)
            {
                var type = a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IModule))).FirstOrDefault();
                if (type != null)
                {
                    var module = (IModule)Activator.CreateInstance(type);
                    ModuleManager.Current.Modules.Add(module, a);
                }
            }
#else
            var assemblies = DependencyContext.Default.GetDefaultAssemblyNames().Where(x => x.Name.StartsWith(MODULE_NAME_PATTERN) || includeModuleNamePattern.Any(i => x.FullName.Contains(i)));

            foreach (var assembly in assemblies)
            {
                try
                {
                    var a = Assembly.Load(assembly);
                    var type = a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IModule))).FirstOrDefault();
                    var typeInfo = type?.GetTypeInfo();
                    if (type != null && typeInfo != null && !typeInfo.IsAbstract && !typeInfo.IsInterface)
                    {
                        var module = (IModule)Activator.CreateInstance(type);
                        ModuleManager.Current.Modules.Add(module, a);
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    throw new Exception(string.Concat("Upps ... cannot load : ", assembly.FullName), ex);
                }
            }
#endif
        }
    }
}
