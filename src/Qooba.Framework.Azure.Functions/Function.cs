using Qooba.Framework.Azure.Functions.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;

namespace Qooba.Framework.Azure.Functions
{
    public class Function
    {
        public static TFunction Create<TFunction>(object traceWriter, params string[] includeModuleNamePattern)
            where TFunction : class
        {
            Bootstrapper.Bootstrapp(includeModuleNamePattern);
            dynamic logger = traceWriter;
            Action<string, Exception, string> errorLog = (message, ex, source) => { logger.Error(message, ex, source); };
            Action<string, string> infoLog = (message, source) => { logger.Info(message, source); };
            Action<string, string> verboseLog = (message, source) => { logger.Verbose(message, source); };
            Action<string, string> warningLog = (message, source) => { logger.Warning(message, source); };
            ContainerManager.Current.RegisterInstance<ITraceWriter>(new TraceWriterWrapper(errorLog, infoLog, verboseLog, warningLog));
            return ContainerManager.Current.Resolve<TFunction>();
        }

        public static TFunction Create<TFunction>(params string[] includeModuleNamePattern)
            where TFunction : class
        {
            Bootstrapper.Bootstrapp(includeModuleNamePattern);
            return ContainerManager.Current.Resolve<TFunction>();
        }
    }
}
