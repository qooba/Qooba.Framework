namespace Qooba.Framework.Abstractions
{
    internal static class ContainerManager
    {
        private static IContainer container;

        public static void SetContainer(IContainer container)
        {
            if (container == null)
            {
                ContainerManager.container = container;
            }
        }

        public static IContainer Container => container;
    }
}
