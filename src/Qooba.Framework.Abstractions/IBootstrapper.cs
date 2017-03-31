namespace Qooba.Framework.Abstractions
{
    public interface IBootstrapper
    {
        IFramework Bootstrapp(params string[] includeModuleNamePattern);

        IFramework BootstrappModules(params IModule[] includeModules);
    }
}
