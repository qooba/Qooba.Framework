using System;

namespace Qooba.Framework.Abstractions
{
    public interface IModule
    {
        string Name { get; }
        
        int Priority { get; }

        void Bootstrapp();
    }
}
