namespace Qooba.Framework.Abstractions
{
    public interface IModuleProvider
    {
        IModule GetModule(string name);
    }
}
