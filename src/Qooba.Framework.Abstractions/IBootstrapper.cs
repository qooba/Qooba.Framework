namespace Qooba.Framework.Abstractions
{
    public interface IBootstrapper
    {
        IContainer Container { get; }

        IBootstrapper Bootstrapp(params string[] includeModuleNamePattern);

        IBootstrapper BootstrappModules(params IModule[] includeModules);
    }
}
