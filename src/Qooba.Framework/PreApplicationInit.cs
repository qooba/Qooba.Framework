#if NET46
#else
using Microsoft.Extensions.DependencyModel;
#endif
using Qooba.Framework.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

//[assembly: PreApplicationStartMethod(typeof(Qooba.Framework.PreApplicationInit), "InitializeModules")]
namespace Qooba.Framework
{
    public class PreApplicationInit
    {
        public const string MODULE_NAME_PATTERN = "Qooba";
        
        public static void InitializeModules(params string[] includeModuleNamePattern)
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
                    if (type != null)
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

            //var assemblies1 = PlatformServices.Default.LibraryManager.GetLibraries().Where(x => x.Name.StartsWith("Qooba")).Select(x => x.Name);
            //var assemblies = assemblyNames.Select(x => Assembly.Load(new AssemblyName(x)));
//#if DNX46
//            var assemblies = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.LibraryManager.GetLibraries().Where(x => x.Name.StartsWith("Qooba")).SelectMany(x => x.Assemblies)
//                .Where(x => x.Name.StartsWith(MODULE_NAME_PATTERN));

//            foreach (var assembly in assemblies)
//            {
//                try
//                {
//                    var a = Assembly.Load(assembly);
//                    var type = a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IModule))).FirstOrDefault();
//                    if (type != null)
//                    {
//                        var module = (IModule)Activator.CreateInstance(type);
//                        ModuleManager.Current.Modules.Add(module, a);
//                    }
//                }
//                catch(ReflectionTypeLoadException ex)
//                {
//                    throw new Exception(string.Concat("Upps ... cannot load : ", assembly.FullName), ex);
//                }
//            }

#endif
        }
    }
}
