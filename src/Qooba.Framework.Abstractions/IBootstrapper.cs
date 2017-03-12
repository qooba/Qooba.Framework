namespace Qooba.Framework.Abstractions
{
    public interface IBootstrapper
    {
        IContainer Container { get; }

        IBootstrapper Bootstrapp(params string[] includeModuleNamePattern);

        IBootstrapper Bootstrapp(params IModule[] includeModules);
    }
}
