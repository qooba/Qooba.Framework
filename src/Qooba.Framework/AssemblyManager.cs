using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Reflection;
using Qooba.Framework.Abstractions;
using System;

namespace Qooba.Framework
{
    internal class AssemblyManager : IAssemblyManager, IAssemblyBootstrapper
    {
        internal static ConcurrentBag<Assembly> Assemblies = new ConcurrentBag<Assembly>();
        
        public IList<Assembly> GetAssemblies() => Assemblies.Distinct().ToList();

        public IAssemblyManager AddAssembly(Func<IAssemblyDescriptor, IAssemblyDescriptor> assemblyDescriptorFactory)
        {
            AssemblyDescriptor descriptor = assemblyDescriptorFactory(new AssemblyDescriptor()) as AssemblyDescriptor;
            descriptor.Assemblies.ToList().ForEach(a => Assemblies.Add(a));
            return this;
        }
    }
}
