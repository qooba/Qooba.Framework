using System;
using System.Reflection;
using Qooba.Framework.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.IO;
#if NET46
#else
using Microsoft.Extensions.DependencyModel;
#endif

namespace Qooba.Framework
{
    internal class AssemblyDescriptor : IAssemblyDescriptor
    {
        internal const string MODULE_NAME_PATTERN = "Qooba";

        public AssemblyDescriptor()
        {
            this.Assemblies = new List<Assembly>();
            this.Patterns = new List<string>() { MODULE_NAME_PATTERN };
        }

        public IList<Assembly> Assemblies { get; }

        public IList<string> Patterns { get; }

        public IAssemblyDescriptor All()
        {
            IEnumerable<AssemblyName> assemblyNames;
#if NET46
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var d = new System.IO.DirectoryInfo(path);
            assemblyNames = d.GetFiles("*.dll", System.IO.SearchOption.AllDirectories)
                .Where(x => this.Patterns.Any(i => x.FullName.Contains(i)))
                .Select(x => AssemblyName.GetAssemblyName(x.FullName)).Where(x => this.Patterns.Any(i => x.FullName.Contains(i)));
#else
            assemblyNames = DependencyContext.Default.GetDefaultAssemblyNames().Where(x => this.Patterns.Any(i => x.FullName.Contains(i)));
#endif
            foreach (var assemblyName in assemblyNames)
            {
                this.Assembly(assemblyName);
            }

            return this;
        }

        public IAssemblyDescriptor All(string assembliesPath)
        {
#if NET46
            var path = System.IO.Path.GetDirectoryName(assembliesPath);
            var d = new System.IO.DirectoryInfo(path);
            var assemblyNames = d.GetFiles("*.dll", System.IO.SearchOption.AllDirectories)
                .Where(x => this.Patterns.Any(i => x.FullName.Contains(i)))
                .Select(x => AssemblyName.GetAssemblyName(x.FullName)).Where(x => this.Patterns.Any(i => x.FullName.Contains(i)));

            foreach (var assemblyName in assemblyNames)
            {
                this.Assembly(assemblyName);
            }
#endif
            return this;
        }

        public IAssemblyDescriptor Assembly(Assembly assembly)
        {
            if (this.Patterns.Any(p => assembly.FullName.StartsWith(p)))
            {
                Assemblies.Add(assembly);
            }

            return this;
        }

        public IAssemblyDescriptor Assembly(AssemblyName assemblyName) => this.Assembly(System.Reflection.Assembly.Load(assemblyName));

        public IAssemblyDescriptor AssemblyPath(string assemblyPath)
        {
#if NET46
            var assemblyName = AssemblyName.GetAssemblyName(assemblyPath);
            this.Assembly(assemblyName);
#endif
            return this;
        }

        public IAssemblyDescriptor Pattern(string assemblyPattern)
        {
            this.Patterns.Add(assemblyPattern);
            return this;
        }

        public IAssemblyDescriptor Pattern(params string[] assemblyPatterns)
        {
            assemblyPatterns.ToList().ForEach(p => this.Patterns.Add(p));
            return this;
        }
    }
}
