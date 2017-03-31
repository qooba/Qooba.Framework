namespace Qooba.Framework.Abstractions
{
    public interface IModuleManager
    {
        IModuleManager AddModule(IModule module);

        IModule GetModule(string name);
    }
}
