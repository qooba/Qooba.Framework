namespace Qooba.Framework.Abstractions
{
    internal static class ContainerManager
    {
        private static IContainer container;

        public static IContainer Container
        {
            get { return container; }
            set { container = value; }
        }
    }
}
