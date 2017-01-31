namespace Qooba.Framework.DependencyInjection.Abstractions
{
    public static class ContainerManager
    {
        public static void SetContainer(IContainer container)
        {
            if (Current == null)
            {
                Current = container;
            }
        }

        public static IContainer Current { get; private set; }
    }
}
