using System;
using System.Reflection;
using Qooba.Framework.Abstractions;
using System.Collections.Generic;
using System.Linq;
#if NET46
#else
using Microsoft.Extensions.DependencyModel;
#endif

namespace Qooba.Framework
{
    internal class AssemblyDescriptor : IAssemblyDescriptor
    {
        internal static IList<Assembly> Assemblies = new List<Assembly>();

        public IAssemblyDescriptor All()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
            toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));
        }

        public IAssemblyDescriptor All(string assembliesPath)
        {
            throw new NotImplementedException();
        }

        public IAssemblyDescriptor Assembly(Assembly assembly)
        {
            Assemblies.Add(assembly);
            return this;
        }

        public IAssemblyDescriptor AssemblyPath(string assemblyPath)
        {
            Assembly

            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var d = new System.IO.DirectoryInfo(path);
            var assemblies = d.GetFiles("*.dll", System.IO.SearchOption.AllDirectories)
                .Where(x => x.FullName.Contains(MODULE_NAME_PATTERN) || includeModuleNamePattern.Any(i => x.FullName.Contains(i)))
                .Select(x => AssemblyName.GetAssemblyName(x.FullName)).Where(x => x.FullName.StartsWith(MODULE_NAME_PATTERN) || includeModuleNamePattern.Any(i => x.FullName.Contains(i)))
                .Select(x => Assembly.Load(x.FullName));
        }

        public IAssemblyDescriptor Pattern(string assemblyPattern)
        {
            throw new NotImplementedException();
        }
        
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
